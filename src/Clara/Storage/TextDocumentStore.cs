using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap? synonymMap;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly ObjectPool<IPhraseTermSource> phraseTermSourcePool;

        public TextDocumentStore(
            TokenEncoder tokenEncoder,
            IAnalyzer analyzer,
            ISynonymMap? synonymMap,
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (tokenDocumentScores is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentScores));
            }

            this.tokenEncoder = tokenEncoder;
            this.analyzer = analyzer;
            this.synonymMap = synonymMap;
            this.tokenDocumentScores = tokenDocumentScores;
            this.phraseTermSourcePool = new ObjectPool<IPhraseTermSource>(() => this.synonymMap?.CreatePhraseTermSource() ?? new PhraseTermSource(this.analyzer.CreateTokenTermSource()));
        }

        public DocumentScoring Search(SearchMode searchMode, string text, Func<Position, float>? positionBoost, ref DocumentResultBuilder documentResultBuilder)
        {
            using var source = this.phraseTermSourcePool.Lease();
            using var termScores = SharedObjectPools.DocumentScores.Lease();

            var documentScores = SharedObjectPools.DocumentScores.Lease();
            var isFirst = true;

            foreach (var term in source.Instance.GetTerms(text))
            {
                termScores.Instance.Clear();

                if (term.Token is Token token)
                {
                    if (this.tokenEncoder.ToReadOnly(token) is string value)
                    {
                        this.Search(value, termScores.Instance);
                    }
                }
                else if (term.Phrases is PhraseGroup phrases)
                {
                    this.Search(phrases, termScores.Instance);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported phrase term value encountered.");
                }

                var boost = ValueCombiner.DefaultBoost;

                if (positionBoost is not null)
                {
                    boost = positionBoost(term.Position);
                }

                if (searchMode == SearchMode.All)
                {
                    if (isFirst)
                    {
                        documentScores.Instance.UnionWith(termScores.Instance, ValueCombiner.Sum, boost);
                        isFirst = false;
                    }
                    else
                    {
                        documentScores.Instance.IntersectWith(termScores.Instance, ValueCombiner.Sum, boost);
                    }

                    if (documentScores.Instance.Count == 0)
                    {
                        break;
                    }
                }
                else
                {
                    documentScores.Instance.UnionWith(termScores.Instance, ValueCombiner.Sum, boost);
                }
            }

            documentResultBuilder.IntersectWith(documentScores.Instance);

            return new DocumentScoring(documentScores);
        }

        private void Search(string value, DictionarySlim<int, float> termScores)
        {
            if (this.tokenEncoder.TryEncode(value, out var tokenId))
            {
                if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                {
                    termScores.UnionWith(documents, ValueCombiner.Sum, ValueCombiner.DefaultBoost);
                }
            }
        }

        private void Search(PhraseGroup phrases, DictionarySlim<int, float> termScores)
        {
            using var phraseScores = SharedObjectPools.DocumentScores.Lease();

            foreach (var phrase in phrases)
            {
                phraseScores.Instance.Clear();

                this.Search(phrase, phraseScores.Instance);

                termScores.UnionWith(phraseScores.Instance, ValueCombiner.Max, ValueCombiner.DefaultBoost);
            }
        }

        private void Search(Phrase phrase, DictionarySlim<int, float> phraseScores)
        {
            var isFirst = true;

            foreach (var term in phrase)
            {
                if (this.tokenEncoder.TryEncode(term, out var tokenId))
                {
                    if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                    {
                        if (isFirst)
                        {
                            phraseScores.UnionWith(documents, ValueCombiner.Sum, ValueCombiner.DefaultBoost);
                            isFirst = false;
                        }
                        else
                        {
                            phraseScores.IntersectWith(documents, ValueCombiner.Sum, ValueCombiner.DefaultBoost);
                        }

                        continue;
                    }
                }

                phraseScores.Clear();

                break;
            }
        }

        private static class ValueCombiner
        {
            public const float DefaultBoost = 1.0f;

            public static float Sum(float current, float value, float boost)
            {
                var boosted = value * boost;

                return current + boosted;
            }

            public static float Max(float current, float value, float boost)
            {
                var boosted = value * boost;

                return current > boosted ? current : boosted;
            }
        }
    }
}
