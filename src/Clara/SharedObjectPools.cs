﻿using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Clara.Utils;

namespace Clara
{
    internal static class SharedObjectPools
    {
        private const int DefaultFilterFacetCount = 10;

        public static ObjectPool<HashSet<Field>> FieldSets { get; } = new(() => new(), x => x.Clear());

        public static ObjectPool<DictionarySlim<int, int>> TokenCounts { get; } = new(() => new());

        public static ObjectPool<DictionarySlim<int, float>> DocumentScores { get; } = new(() => new(), sizeFactor: 3);

        public static ObjectPool<HashSetSlim<int>> DocumentSets { get; } = new(() => new(), sizeFactor: 4 + DefaultFilterFacetCount);

        public static ObjectPool<HashSetSlim<int>> FilteredTokens { get; } = new(() => new());

        public static ObjectPool<HashSetSlim<string>> FilterValues { get; } = new(() => new(), sizeFactor: 1 + DefaultFilterFacetCount);

        public static ObjectPool<ListSlim<int>> Documents { get; } = new(() => new());

        public static ObjectPool<ListSlim<DocumentValue<float>>> ScoredDocuments { get; } = new(() => new());

        public static ObjectPool<ListSlim<FacetExpression>> FacetExpressions { get; } = new(() => new());

        public static ObjectPool<ListSlim<FilterExpression>> FilterExpressions { get; } = new(() => new(), sizeFactor: 2);

        public static ObjectPool<ListSlim<FacetResult>> FacetResults { get; } = new(() => new());

        public static ObjectPool<ListSlim<HierarchyFacetValue>> HierarchyFacetValues { get; } = new(() => new());

        public static ObjectPool<ListSlim<KeywordFacetValue>> KeywordFacetValues { get; } = new(() => new(), sizeFactor: DefaultFilterFacetCount);

        public static ObjectPool<ListSlim<DocumentResultBuilderFacet>> QueryResultBuilderFacets { get; } = new(() => new(), sizeFactor: DefaultFilterFacetCount);

        public static ObjectPool<FilterExpressionComparer> FilterExpressionComparers { get; } = new(() => new());
    }

    internal static class SharedObjectPools<TValue>
        where TValue : struct, IComparable<TValue>
    {
        public static ObjectPool<ListSlim<DocumentValue<TValue>>> SortedDocumentLists { get; } = new(() => new());
    }
}
