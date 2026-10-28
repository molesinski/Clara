using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private const float DefaultBoost = 1f;

        private readonly TokenEncoder tokenEncoder;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap? synonymMap;
        private readonly ScoreAggregation searchScoreAggregation;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly ObjectPoolSlim<IPhraseTermSource> phraseTermSourcePool;
        private readonly TokenExpansionStore? tokenExpansionStore;

        public TextDocumentStore(
            TokenEncoder tokenEncoder,
            IAnalyzer analyzer,
            ISynonymMap? synonymMap,
            ScoreAggregation searchScoreAggregation,
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            TokenExpansionStore? tokenExpansionStore = null)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (searchScoreAggregation is null)
            {
                throw new ArgumentNullException(nameof(searchScoreAggregation));
            }

            if (tokenDocumentScores is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentScores));
            }

            this.tokenEncoder = tokenEncoder;
            this.analyzer = analyzer;
            this.synonymMap = synonymMap;
            this.searchScoreAggregation = searchScoreAggregation;
            this.tokenDocumentScores = tokenDocumentScores;
            this.tokenExpansionStore = tokenExpansionStore;
            this.phraseTermSourcePool = new ObjectPoolSlim<IPhraseTermSource>(() => this.synonymMap?.CreatePhraseTermSource() ?? new PhraseTermSource(this.analyzer.CreateTokenTermSource()));
        }

        public DocumentScoring Search(SearchMode searchMode, string text, Func<Position, float>? positionBoost, ScoreAggregation? searchScoreAggregation, FuzzyMode fuzzyMode = FuzzyMode.None, PrefixMode prefixMode = PrefixMode.None)
        {
            var isExpanding = fuzzyMode != FuzzyMode.None || prefixMode != PrefixMode.None;

            if (isExpanding && this.tokenExpansionStore is null)
            {
                throw new InvalidOperationException("Field does not support token expansion.");
            }

            using var source = this.phraseTermSourcePool.Lease();
            using var termScores = SharedObjectPools.DocumentScores.Lease();
            using var surfaceTerms = isExpanding ? SharedObjectPools.SurfaceTerms.Lease() : default;

            var documentScores = SharedObjectPools.DocumentScores.Lease();

            try
            {
                var isFirst = true;
                var lastPosition = -1;
                var isLastPrefix = false;

                searchScoreAggregation ??= this.searchScoreAggregation;

                if (this.tokenExpansionStore is not null && isExpanding)
                {
                    lastPosition = this.tokenExpansionStore.GetSurfaceTerms(text, surfaceTerms.Instance);
                    isLastPrefix = prefixMode == PrefixMode.Last && text.Length > 0 && !char.IsWhiteSpace(text[text.Length - 1]);
                }

                foreach (var term in source.Instance.GetTerms(text))
                {
                    termScores.Instance.Clear();

                    if (term.Token is Token token)
                    {
                        if (this.tokenEncoder.ToReadOnly(token) is string value)
                        {
                            this.Search(value, termScores.Instance, searchScoreAggregation);
                        }
                    }
                    else if (term.Phrases is PhraseGroup phrases)
                    {
                        this.Search(phrases, termScores.Instance, searchScoreAggregation);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported phrase term value encountered.");
                    }

                    if (this.tokenExpansionStore is not null && isExpanding && term.Position.Start == term.Position.End)
                    {
                        if (surfaceTerms.Instance.TryGetValue(term.Position.Start, out var surface))
                        {
                            var isPrefix = prefixMode == PrefixMode.All || (isLastPrefix && term.Position.Start == lastPosition);

                            var isFuzzy = fuzzyMode == FuzzyMode.Always;

                            if (isFuzzy || isPrefix)
                            {
                                this.SearchCandidates(surface, isFuzzy, isPrefix, termScores.Instance);
                            }

                            if (fuzzyMode == FuzzyMode.Fallback && termScores.Instance.Count == 0)
                            {
                                this.SearchCandidates(surface, isFuzzy: true, isPrefix, termScores.Instance);
                            }
                        }
                    }

                    var boost = DefaultBoost;

                    if (positionBoost is not null)
                    {
                        boost = positionBoost(term.Position);
                    }

                    if (searchMode == SearchMode.All)
                    {
                        if (isFirst)
                        {
                            documentScores.Instance.UnionWith(termScores.Instance, searchScoreAggregation, boost);
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.Instance.IntersectWith(termScores.Instance, searchScoreAggregation, boost);
                        }

                        if (documentScores.Instance.Count == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        documentScores.Instance.UnionWith(termScores.Instance, searchScoreAggregation, boost);
                    }
                }

                return new DocumentScoring(documentScores);
            }
            catch
            {
                documentScores.Dispose();
                throw;
            }
        }

        private void SearchCandidates(string surface, bool isFuzzy, bool isPrefix, DictionarySlim<int, float> termScores)
        {
            using var candidates = SharedObjectPools.DocumentScores.Lease();

            this.tokenExpansionStore!.GetCandidates(surface, isFuzzy, isPrefix, candidates.Instance);

            foreach (var candidate in candidates.Instance)
            {
                if (this.tokenDocumentScores.TryGetValue(candidate.Key, out var tokenDocuments))
                {
                    termScores.UnionWith(tokenDocuments, ScoreAggregation.Max, candidate.Value);
                }
            }
        }

        private void Search(string value, DictionarySlim<int, float> termScores, ScoreAggregation searchScoreAggregation)
        {
            if (this.tokenEncoder.TryEncode(value, out var tokenId))
            {
                if (this.tokenDocumentScores.TryGetValue(tokenId, out var tokenDocuments))
                {
                    termScores.UnionWith(tokenDocuments, searchScoreAggregation, DefaultBoost);
                }
            }
        }

        private void Search(PhraseGroup phrases, DictionarySlim<int, float> termScores, ScoreAggregation searchScoreAggregation)
        {
            using var phraseScores = SharedObjectPools.DocumentScores.Lease();

            foreach (var phrase in phrases)
            {
                phraseScores.Instance.Clear();

                this.Search(phrase, phraseScores.Instance, searchScoreAggregation);

                termScores.UnionWith(phraseScores.Instance, ScoreAggregation.Max, DefaultBoost);
            }
        }

        private void Search(Phrase phrase, DictionarySlim<int, float> phraseScores, ScoreAggregation searchScoreAggregation)
        {
            var isFirst = true;

            foreach (var term in phrase)
            {
                if (this.tokenEncoder.TryEncode(term, out var tokenId))
                {
                    if (this.tokenDocumentScores.TryGetValue(tokenId, out var tokenDocuments))
                    {
                        if (isFirst)
                        {
                            phraseScores.UnionWith(tokenDocuments, searchScoreAggregation, DefaultBoost);
                            isFirst = false;
                        }
                        else
                        {
                            phraseScores.IntersectWith(tokenDocuments, searchScoreAggregation, DefaultBoost);
                        }

                        continue;
                    }
                }

                phraseScores.Clear();

                break;
            }
        }
    }
}
