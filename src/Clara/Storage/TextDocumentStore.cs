using Clara.Analysis;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private static readonly string InvalidToken = string.Concat("__INVALID__", Guid.NewGuid().ToString("N"));

        private readonly TokenEncoder tokenEncoder;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap? synonymMap;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly ObjectPool<ITokenTermSource> tokenTermSourcePool;

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
            this.tokenTermSourcePool = new ObjectPool<ITokenTermSource>(() => this.synonymMap?.CreateSearchTokenTermSource() ?? this.analyzer.CreateTokenTermSource());
        }

        public static DocumentScoring Search(ListSlim<TextSearchFieldStore> stores, SearchMode searchMode, string text, ref DocumentResultBuilder documentResultBuilder)
        {
            using var terms = SharedObjectPools.SearchTerms.Lease();
            using var termStores = SharedObjectPools.SearchTermStoreIndexes.Lease();
            using var positionScores = SharedObjectPools.DocumentScores.Lease();
            using var scoreValueCombiner = SharedObjectPools.ScoreValueCombiners.Lease();

            var documentScores = SharedObjectPools.DocumentScores.Lease();
            var lastAnalyzedStore = default(TextDocumentStore?);

            SortByEqualAnalysis(stores);

            for (var storeIndex = 0; storeIndex < stores.Count; storeIndex++)
            {
                var store = stores[storeIndex];

                if (!store.Store.AnalysisEquals(lastAnalyzedStore))
                {
                    terms.Instance.Clear();

                    store.Store.GetSearchTerms(text, terms.Instance);

                    lastAnalyzedStore = store.Store;
                }

                foreach (var term in terms.Instance)
                {
                    termStores.Instance.Add(new SearchTermStoreIndex(term, storeIndex));
                }
            }

            var span = termStores.Instance.AsSpan();
            var start = int.MaxValue;
            var end = int.MinValue;

            for (var i = 0; i < span.Length; i++)
            {
                ref var term = ref span[i];

                start = start < term.SearchTerm.Position.Start ? start : term.SearchTerm.Position.Start;
                end = end > term.SearchTerm.Position.End ? end : term.SearchTerm.Position.End;
            }

            var isFirst = true;

            for (var position = start; position <= end; position++)
            {
                positionScores.Instance.Clear();

                var coveredStores = 0;

                for (var storeIndex = 0; storeIndex < stores.Count; storeIndex++)
                {
                    var isCovered = false;

                    for (var i = 0; i < span.Length; i++)
                    {
                        ref var termStore = ref span[i];

                        if (termStore.StoreIndex == storeIndex && termStore.SearchTerm.Position.Overlaps(position))
                        {
                            var store = stores[storeIndex];
                            var boost = store.Boost;

                            if (position != termStore.SearchTerm.Position.Start)
                            {
                                boost = 0;
                            }

                            scoreValueCombiner.Instance.Initialize(boost);

                            store.Store.Search(termStore.SearchTerm, positionScores.Instance, scoreValueCombiner.Instance);

                            if (!isCovered)
                            {
                                coveredStores++;
                                isCovered = true;
                            }
                        }
                    }
                }

                if (coveredStores > 0)
                {
                    if (searchMode == SearchMode.All)
                    {
                        if (isFirst)
                        {
                            documentScores.Instance.UnionWith(positionScores.Instance, ScoreValueCombiner.Default);
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.Instance.IntersectWith(positionScores.Instance, ScoreValueCombiner.Default);
                        }

                        if (documentScores.Instance.Count == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        documentScores.Instance.UnionWith(positionScores.Instance, ScoreValueCombiner.Default);
                    }
                }
            }

            documentResultBuilder.IntersectWith(documentScores.Instance);

            return new DocumentScoring(documentScores);
        }

        private static void SortByEqualAnalysis(ListSlim<TextSearchFieldStore> searchFieldStores)
        {
            var span = searchFieldStores.AsSpan();
            var length = span.Length;

            for (var i = 0; i < length - 1; i++)
            {
                ref var a = ref span[i];

                for (var j = i + 1; j < length; j++)
                {
                    ref var b = ref span[j];

                    if (a.Store.AnalysisEquals(b.Store))
                    {
                        if (j != i + 1)
                        {
                            var tmp = b;
                            span[j] = span[i + 1];
                            span[i + 1] = tmp;
                        }

                        break;
                    }
                }
            }
        }

        private void Search(SearchTerm term, DictionarySlim<int, float> documentScores, IValueCombiner<float> scoreCombiner)
        {
            if (this.tokenEncoder.TryEncode(term.Token, out var tokenId))
            {
                if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                {
                    documentScores.UnionWith(documents, scoreCombiner);
                }
            }
        }

        private void GetSearchTerms(string text, ListSlim<SearchTerm> terms)
        {
            using var tokenTermSource = this.tokenTermSourcePool.Lease();

            foreach (var term in tokenTermSource.Instance.GetTerms(text))
            {
                var token = this.tokenEncoder.ToReadOnly(term.Token) ?? InvalidToken;

                terms.Add(new SearchTerm(token, term.Position));
            }
        }

        private bool AnalysisEquals(TextDocumentStore? other)
        {
            return other is not null
                && this.analyzer == other.analyzer
                && this.synonymMap == other.synonymMap;
        }
    }
}
