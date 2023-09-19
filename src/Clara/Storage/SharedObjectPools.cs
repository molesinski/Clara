using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    public static class SharedObjectPools
    {
        internal static ObjectPool<DictionarySlim<int, int>> TokenCounts { get; } = new(() => new(), x => x.Clear());

        internal static ObjectPool<DictionarySlim<int, float>> DocumentScores { get; } = new(() => new(), x => x.Clear(), size: 3);

        internal static ObjectPool<HashSetSlim<int>> DocumentSets { get; } = new(() => new(), x => x.Clear(), size: 12);

        internal static ObjectPool<HashSetSlim<Field>> FieldSets { get; } = new(() => new(), x => x.Clear());

        internal static ObjectPool<HashSetSlim<string>> SelectedValues { get; } = new(() => new(), x => x.Clear());

        internal static ObjectPool<HashSetSlim<int>> FilteredTokens { get; } = new(() => new(), x => x.Clear());

        internal static ObjectPool<ListSlim<FilterExpression>> FilterExpressions { get; } = new(() => new(), x => x.Clear());

        internal static ObjectPool<ListSlim<FacetResult>> FacetResults { get; } = new(() => new(), x => x.Clear());

        internal static ObjectPool<ListSlim<HierarchyFacetValue>> HierarchyFacetValues { get; } = new(() => new(), x => x.Clear(), size: 3);

        internal static ObjectPool<ListSlim<KeywordFacetValue>> KeywordFacetValues { get; } = new(() => new(), x => x.Clear(), size: 12);

        internal static ObjectPool<ListSlim<DocumentFacetSet>> DocumentFacetSets { get; } = new(() => new(), x => x.Clear(), size: 12);
    }
}
