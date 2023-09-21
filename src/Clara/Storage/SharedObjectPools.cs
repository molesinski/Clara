using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    public static class SharedObjectPools
    {
        internal static ObjectPool<DictionarySlim<int, int>> TokenCounts { get; } = new(() => new());

        internal static ObjectPool<DictionarySlim<int, float>> DocumentScores { get; } = new(() => new(), sizeFactor: 3);

        internal static ObjectPool<HashSetSlim<int>> DocumentSets { get; } = new(() => new(), sizeFactor: 12);

        internal static ObjectPool<HashSetSlim<Field>> FieldSets { get; } = new(() => new());

        internal static ObjectPool<HashSetSlim<string>> SelectedValues { get; } = new(() => new());

        internal static ObjectPool<HashSetSlim<int>> FilteredTokens { get; } = new(() => new());

        internal static ObjectPool<ListSlim<int>> DocumentLists { get; } = new(() => new());

        internal static ObjectPool<ListSlim<FilterExpression>> FilterExpressions { get; } = new(() => new());

        internal static ObjectPool<ListSlim<FacetResult>> FacetResults { get; } = new(() => new());

        internal static ObjectPool<ListSlim<HierarchyFacetValue>> HierarchyFacetValues { get; } = new(() => new(), sizeFactor: 3);

        internal static ObjectPool<ListSlim<KeywordFacetValue>> KeywordFacetValues { get; } = new(() => new(), sizeFactor: 12);

        internal static ObjectPool<ListSlim<DocumentResultBuilderFacet>> QueryResultBuilderFacets { get; } = new(() => new(), sizeFactor: 12);
    }
}
