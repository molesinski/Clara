using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal static class SharedObjectPools
    {
        public static ObjectPool<DictionarySlim<int, int>> TokenCounts { get; } = new(() => new());

        public static ObjectPool<DictionarySlim<int, float>> DocumentScores { get; } = new(() => new(), sizeFactor: 3);

        public static ObjectPool<HashSetSlim<int>> DocumentSets { get; } = new(() => new(), sizeFactor: 12);

        public static ObjectPool<HashSetSlim<Field>> FieldSets { get; } = new(() => new());

        public static ObjectPool<HashSetSlim<string>> SelectedValues { get; } = new(() => new());

        public static ObjectPool<HashSetSlim<string>> TokenSets { get; } = new(() => new());

        public static ObjectPool<HashSetSlim<int>> FilteredTokens { get; } = new(() => new());

        public static ObjectPool<ListSlim<int>> DocumentLists { get; } = new(() => new());

        public static ObjectPool<ListSlim<FilterExpression>> FilterExpressions { get; } = new(() => new());

        public static ObjectPool<ListSlim<FacetResult>> FacetResults { get; } = new(() => new());

        public static ObjectPool<ListSlim<HierarchyFacetValue>> HierarchyFacetValues { get; } = new(() => new(), sizeFactor: 3);

        public static ObjectPool<ListSlim<KeywordFacetValue>> KeywordFacetValues { get; } = new(() => new(), sizeFactor: 12);

        public static ObjectPool<ListSlim<DocumentResultBuilderFacet>> QueryResultBuilderFacets { get; } = new(() => new(), sizeFactor: 12);
    }
}
