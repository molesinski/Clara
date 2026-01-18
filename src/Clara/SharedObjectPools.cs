using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Clara.Utils;

namespace Clara
{
    internal static class SharedObjectPools
    {
        private const int DefaultFilterFacetCount = 10;

        public static ObjectPoolSlim<HashSet<Field>> FieldSets { get; } = new(() => new(), x => x.Clear());

        public static ObjectPoolSlim<DictionarySlim<int, int>> TokenCounts { get; } = new(() => new());

        public static ObjectPoolSlim<DictionarySlim<int, float>> DocumentScores { get; } = new(() => new(), sizeFactor: 3);

        public static ObjectPoolSlim<HashSetSlim<int>> DocumentSets { get; } = new(() => new(), sizeFactor: 4 + DefaultFilterFacetCount);

        public static ObjectPoolSlim<HashSetSlim<int>> FilteredTokens { get; } = new(() => new());

        public static ObjectPoolSlim<HashSetSlim<string>> FilterValues { get; } = new(() => new(), sizeFactor: 1 + DefaultFilterFacetCount);

        public static ObjectPoolSlim<ListSlim<int>> Documents { get; } = new(() => new());

        public static ObjectPoolSlim<ListSlim<DocumentValue<float>>> ScoredDocuments { get; } = new(() => new());

        public static ObjectPoolSlim<ListSlim<FacetExpression>> FacetExpressions { get; } = new(() => new());

        public static ObjectPoolSlim<ListSlim<FilterExpression>> FilterExpressions { get; } = new(() => new(), sizeFactor: 2);

        public static ObjectPoolSlim<ListSlim<FacetResult>> FacetResults { get; } = new(() => new());

        public static ObjectPoolSlim<ListSlim<HierarchyFacetValue>> HierarchyFacetValues { get; } = new(() => new());

        public static ObjectPoolSlim<ListSlim<KeywordFacetValue>> KeywordFacetValues { get; } = new(() => new(), sizeFactor: DefaultFilterFacetCount);

        public static ObjectPoolSlim<ListSlim<DocumentResultBuilderFacet>> QueryResultBuilderFacets { get; } = new(() => new(), sizeFactor: DefaultFilterFacetCount);

        public static ObjectPoolSlim<FilterExpressionComparer> FilterExpressionComparers { get; } = new(() => new());
    }

    internal static class SharedObjectPools<TValue>
        where TValue : struct, IComparable<TValue>
    {
        public static ObjectPoolSlim<ListSlim<DocumentValue<TValue>>> SortedDocumentLists { get; } = new(() => new());
    }
}
