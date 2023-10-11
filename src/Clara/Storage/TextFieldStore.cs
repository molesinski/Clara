using Clara.Analysis;
using Clara.Analysis.MatchExpressions;
using Clara.Analysis.Synonyms;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStore : FieldStore
    {
        private static readonly string InvalidToken = string.Concat("__INVALID__", Guid.NewGuid().ToString("N"));

        private readonly TokenEncoder tokenEncoder;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap? synonymMap;
        private readonly TextDocumentStore textDocumentStore;

        public TextFieldStore(
            TokenEncoder tokenEncoder,
            IAnalyzer analyzer,
            ISynonymMap? synonymMap,
            TextDocumentStore textDocumentStore)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (textDocumentStore is null)
            {
                throw new ArgumentNullException(nameof(textDocumentStore));
            }

            this.tokenEncoder = tokenEncoder;
            this.analyzer = analyzer;
            this.synonymMap = synonymMap;
            this.textDocumentStore = textDocumentStore;
        }

        public static DocumentScoring Search(SearchMode searchMode, string text, ListSlim<SearchFieldStore> searchFieldStores, ref DocumentResultBuilder documentResultBuilder)
        {
            if (searchMode == SearchMode.Any || searchFieldStores.Count == 1)
            {
                using var terms = SharedObjectPools.SearchTerms.Lease();
                using var tempScores = SharedObjectPools.DocumentScores.Lease();
                using var scoreCombiner = SharedObjectPools.ScoreCombiners.Lease();

                var documentScores = SharedObjectPools.DocumentScores.Lease();

                var lastAnalyzer = default(IAnalyzer?);
                var lastSynonymMap = default(ISynonymMap?);

                Sort(searchFieldStores);

                foreach (var searchFieldStore in searchFieldStores)
                {
                    tempScores.Instance.Clear();

                    var store = searchFieldStore.Store;
                    var boost = searchFieldStore.SearchField.Boost;

                    if (store.analyzer != lastAnalyzer || store.synonymMap != lastSynonymMap)
                    {
                        terms.Instance.Clear();

                        foreach (var term in store.analyzer.GetTerms(text))
                        {
                            var token = store.tokenEncoder.ToReadOnly(term.Token)
                                     ?? store.synonymMap?.ToReadOnly(term.Token)
                                     ?? InvalidToken;

                            terms.Instance.Add(new SearchTerm(term.Ordinal, token));
                        }

                        store.synonymMap?.Process(searchMode, terms.Instance);

                        lastAnalyzer = store.analyzer;
                        lastSynonymMap = store.synonymMap;
                    }

                    store.textDocumentStore.Search(searchMode, terms.Instance, tempScores.Instance);

                    scoreCombiner.Instance.Initialize(ScoreAggregation.Sum, boost);

                    documentScores.Instance.UnionWith(tempScores.Instance, scoreCombiner.Instance);
                }

                documentResultBuilder.IntersectWith(documentScores.Instance);

                return new DocumentScoring(documentScores);
            }
            else
            {
                using var terms = SharedObjectPools.SearchTerms.Lease();
                using var termIndexes = SharedObjectPools.SearchTermStoreIndexes.Lease();
                using var tempScores = SharedObjectPools.DocumentScores.Lease();
                using var tempScores2 = SharedObjectPools.DocumentScores.Lease();
                using var scoreCombiner = SharedObjectPools.ScoreCombiners.Lease();

                var documentScores = SharedObjectPools.DocumentScores.Lease();

                var lastAnalyzer = default(IAnalyzer?);
                var lastSynonymMap = default(ISynonymMap?);

                Sort(searchFieldStores);

                for (var i = 0; i < searchFieldStores.Count; i++)
                {
                    var store = searchFieldStores[i].Store;

                    if (store.analyzer != lastAnalyzer || store.synonymMap != lastSynonymMap)
                    {
                        terms.Instance.Clear();

                        foreach (var term in store.analyzer.GetTerms(text))
                        {
                            var token = store.tokenEncoder.ToReadOnly(term.Token)
                                     ?? store.synonymMap?.ToReadOnly(term.Token)
                                     ?? InvalidToken;

                            terms.Instance.Add(new SearchTerm(term.Ordinal, token));
                        }

                        store.synonymMap?.Process(searchMode, terms.Instance);

                        lastAnalyzer = store.analyzer;
                        lastSynonymMap = store.synonymMap;
                    }

                    foreach (var term in terms.Instance)
                    {
                        termIndexes.Instance.Add(new SearchTermStoreIndex(term, i));
                    }
                }

                termIndexes.Instance.Sort(SearchTermStoreIndexComparer.Instance);

                var isFirst = true;

                foreach (var ordinal in new SearchTermStoreIndexEnumerable(termIndexes.Instance))
                {
                    tempScores.Instance.Clear();

                    foreach (var storeIndex in ordinal)
                    {
                        terms.Instance.Clear();
                        tempScores2.Instance.Clear();

                        foreach (var term in storeIndex)
                        {
                            terms.Instance.Add(term);
                        }

                        var searchFieldStore = searchFieldStores[storeIndex.StoreIndex];
                        var store = searchFieldStore.Store;
                        var boost = searchFieldStore.SearchField.Boost;

                        store.textDocumentStore.Search(SearchMode.Any, terms.Instance, tempScores2.Instance);

                        scoreCombiner.Instance.Initialize(ScoreAggregation.Sum, boost);

                        tempScores.Instance.UnionWith(tempScores2.Instance, scoreCombiner.Instance);
                    }

                    if (isFirst)
                    {
                        documentScores.Instance.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
                        isFirst = false;
                    }
                    else
                    {
                        documentScores.Instance.IntersectWith(tempScores.Instance, ScoreCombiner.Sum);
                    }

                    if (documentScores.Instance.Count == 0)
                    {
                        break;
                    }
                }

                documentResultBuilder.IntersectWith(documentScores.Instance);

                return new DocumentScoring(documentScores);
            }
        }

        private static void Sort(ListSlim<SearchFieldStore> searchFieldStores)
        {
            var count = searchFieldStores.Count;

            for (var i = 0; i < count - 1; i++)
            {
                var a = searchFieldStores[i];

                for (var j = i + 1; j < count; j++)
                {
                    var b = searchFieldStores[j];

                    if (a.Store.analyzer == b.Store.analyzer && a.Store.synonymMap == b.Store.synonymMap)
                    {
                        if (j != i + 1)
                        {
                            searchFieldStores[j] = searchFieldStores[i + 1];
                            searchFieldStores[i + 1] = b;
                        }

                        break;
                    }
                }
            }
        }
    }
}
