using Clara.Analysis;
using Clara.Analysis.MatchExpressions;
using Clara.Analysis.Synonyms;
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
        }

        public static DocumentScoring Search(ListSlim<SearchFieldStore> searchFieldStores, SearchMode searchMode, string text, ref DocumentResultBuilder documentResultBuilder)
        {
            if (searchMode == SearchMode.Any || searchFieldStores.Count == 1)
            {
                using var terms = SharedObjectPools.SearchTerms.Lease();
                using var tempScores = SharedObjectPools.DocumentScores.Lease();
                using var scoreCombiner = SharedObjectPools.ScoreCombiners.Lease();

                var documentScores = SharedObjectPools.DocumentScores.Lease();
                var lastAnalyzedStore = default(TextDocumentStore?);

                SortByEqualAnalysis(searchFieldStores);

                foreach (var searchFieldStore in searchFieldStores)
                {
                    tempScores.Instance.Clear();

                    var store = searchFieldStore.Store;
                    var boost = searchFieldStore.SearchField.Boost;

                    if (!store.AnalysisEquals(lastAnalyzedStore))
                    {
                        terms.Instance.Clear();

                        store.GetSearchTerms(searchMode, text, terms.Instance);

                        lastAnalyzedStore = store;
                    }

                    store.Search(searchMode, terms.Instance, tempScores.Instance);

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
                var lastAnalyzedStore = default(TextDocumentStore?);

                SortByEqualAnalysis(searchFieldStores);

                for (var i = 0; i < searchFieldStores.Count; i++)
                {
                    var store = searchFieldStores[i].Store;

                    if (!store.AnalysisEquals(lastAnalyzedStore))
                    {
                        terms.Instance.Clear();

                        store.GetSearchTerms(searchMode, text, terms.Instance);

                        lastAnalyzedStore = store;
                    }

                    foreach (var term in terms.Instance)
                    {
                        termIndexes.Instance.Add(new SearchTermStoreIndex(term, i));
                    }
                }

                termIndexes.Instance.Sort(SearchTermStoreIndexComparer.Instance);

                var isFirst = true;

                foreach (var positionRange in new SearchTermStoreIndexEnumerable(termIndexes.Instance))
                {
                    tempScores.Instance.Clear();

                    foreach (var storeRange in positionRange)
                    {
                        terms.Instance.Clear();
                        tempScores2.Instance.Clear();

                        foreach (var term in storeRange)
                        {
                            terms.Instance.Add(term);
                        }

                        var searchFieldStore = searchFieldStores[storeRange.StoreIndex];
                        var store = searchFieldStore.Store;
                        var boost = searchFieldStore.SearchField.Boost;

                        store.Search(searchMode, terms.Instance, tempScores2.Instance);

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

            static void SortByEqualAnalysis(ListSlim<SearchFieldStore> searchFieldStores)
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
                                var tmp = span[j];
                                span[j] = span[i + 1];
                                span[i + 1] = tmp;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void Search(SearchMode mode, ListSlim<SearchTerm> terms, DictionarySlim<int, float> documentScores)
        {
            if (mode == SearchMode.Any)
            {
                foreach (var term in terms)
                {
                    if (term.Token is string token)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentScores.UnionWith(documents, ScoreCombiner.Sum);
                            }
                        }
                    }
                    else if (term.Expression is MatchExpression expression)
                    {
                        using var tempScores = SharedObjectPools.DocumentScores.Lease();

                        this.Search(expression, tempScores.Instance);

                        documentScores.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
                    }
                }
            }
            else
            {
                var isFirst = true;

                foreach (var term in terms)
                {
                    if (term.Token is string token)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                if (isFirst)
                                {
                                    documentScores.UnionWith(documents, ScoreCombiner.Sum);
                                    isFirst = false;
                                }
                                else
                                {
                                    documentScores.IntersectWith(documents, ScoreCombiner.Sum);
                                }
                            }
                            else
                            {
                                documentScores.Clear();
                            }
                        }
                        else
                        {
                            documentScores.Clear();
                        }
                    }
                    else if (term.Expression is MatchExpression expression)
                    {
                        using var tempScores = SharedObjectPools.DocumentScores.Lease();

                        this.Search(expression, tempScores.Instance);

                        if (tempScores.Instance.Count > 0)
                        {
                            if (isFirst)
                            {
                                documentScores.UnionWith(tempScores.Instance, ScoreCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                documentScores.IntersectWith(tempScores.Instance, ScoreCombiner.Sum);
                            }
                        }
                        else
                        {
                            documentScores.Clear();
                        }
                    }

                    if (documentScores.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        private void Search(MatchExpression matchExpression, DictionarySlim<int, float> documentScores)
        {
            if (matchExpression is AnyTokensMatchExpression anyTokensMatchExpression)
            {
                for (var i = 0; i < anyTokensMatchExpression.Tokens.Count; i++)
                {
                    if (this.tokenEncoder.TryEncode(anyTokensMatchExpression.Tokens[i], out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            documentScores.UnionWith(documents, ScoreCombiner.For(anyTokensMatchExpression.ScoreAggregation));

                            if (anyTokensMatchExpression.IsLazy)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else if (matchExpression is AllTokensMatchExpression allTokensMatchExpression)
            {
                var isFirst = true;

                for (var i = 0; i < allTokensMatchExpression.Tokens.Count; i++)
                {
                    if (this.tokenEncoder.TryEncode(allTokensMatchExpression.Tokens[i], out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                documentScores.UnionWith(documents, ScoreCombiner.For(allTokensMatchExpression.ScoreAggregation));
                                isFirst = false;
                            }
                            else
                            {
                                documentScores.IntersectWith(documents, ScoreCombiner.For(allTokensMatchExpression.ScoreAggregation));
                            }
                        }
                        else
                        {
                            documentScores.Clear();
                        }
                    }
                    else
                    {
                        documentScores.Clear();
                    }

                    if (documentScores.Count == 0)
                    {
                        break;
                    }
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                for (var i = 0; i < orMatchExpression.Expressions.Count; i++)
                {
                    tempScores.Instance.Clear();

                    this.Search(orMatchExpression.Expressions[i], tempScores.Instance);

                    documentScores.UnionWith(tempScores.Instance, ScoreCombiner.For(orMatchExpression.ScoreAggregation));

                    if (orMatchExpression.IsLazy)
                    {
                        if (tempScores.Instance.Count > 0)
                        {
                            break;
                        }
                    }
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = SharedObjectPools.DocumentScores.Lease();

                for (var i = 0; i < andMatchExpression.Expressions.Count; i++)
                {
                    tempScores.Instance.Clear();

                    this.Search(andMatchExpression.Expressions[i], tempScores.Instance);

                    if (tempScores.Instance.Count > 0)
                    {
                        if (isFirst)
                        {
                            documentScores.UnionWith(tempScores.Instance, ScoreCombiner.For(andMatchExpression.ScoreAggregation));
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.IntersectWith(tempScores.Instance, ScoreCombiner.For(andMatchExpression.ScoreAggregation));
                        }
                    }
                    else
                    {
                        documentScores.Clear();
                    }

                    if (documentScores.Count == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }

        private void GetSearchTerms(SearchMode searchMode, string text, ListSlim<SearchTerm> terms)
        {
            foreach (var term in this.analyzer.GetTerms(text))
            {
                var token = this.tokenEncoder.ToReadOnly(term.Token)
                         ?? this.synonymMap?.ToReadOnly(term.Token)
                         ?? InvalidToken;

                terms.Add(new SearchTerm(term.Position, token));
            }

            this.synonymMap?.Process(searchMode, terms);
        }

        private bool AnalysisEquals(TextDocumentStore? other)
        {
            return other is not null
                && this.analyzer == other.analyzer
                && this.synonymMap == other.synonymMap;
        }
    }
}
