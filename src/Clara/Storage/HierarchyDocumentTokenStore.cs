using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyDocumentTokenStore
    {
        private readonly HashSet<string> rootSet;
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> documentTokens;
        private readonly DictionarySlim<int, HashSetSlim<int>> parentChildren;

        public HierarchyDocumentTokenStore(
            string root,
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, HashSetSlim<int>> documentTokens,
            DictionarySlim<int, HashSetSlim<int>> parentChildren)
        {
            if (root is null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documentTokens is null)
            {
                throw new ArgumentNullException(nameof(documentTokens));
            }

            if (parentChildren is null)
            {
                throw new ArgumentNullException(nameof(parentChildren));
            }

            this.rootSet = new HashSet<string> { root };
            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
            this.parentChildren = parentChildren;
        }

        public FieldFacetResult? Facet(HierarchyFacetExpression hierarchyFacetExpression, FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            var selectedValues = this.rootSet;

            if (filterExpression is TokenFilterExpression tokenFilterExpression)
            {
                if (tokenFilterExpression.ValuesExpression.Values.Count > 0)
                {
                    selectedValues = new HashSet<string>(tokenFilterExpression.ValuesExpression.Values);
                }
            }

            using var filteredTokens = new PooledHashSet<int>(Allocator.ArrayPool);

            foreach (var selectedToken in selectedValues)
            {
                if (this.tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    filteredTokens.Add(parentId);

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        filteredTokens.UnionWith(children);
                    }
                }
            }

            using var tokenCounts = new PooledDictionary<int, int>(Allocator.ArrayPool, capacity: filteredTokens.Count);

            foreach (var documentId in documents)
            {
                if (this.documentTokens.TryGetValue(documentId, out var tokenIds))
                {
                    foreach (var tokenId in tokenIds)
                    {
                        if (filteredTokens.Contains(tokenId))
                        {
                            ref var count = ref tokenCounts.GetValueRefOrAddDefault(tokenId, out _);

                            count++;
                        }
                    }
                }
            }

#pragma warning disable CA2000 // Dispose objects before losing scope
            var values = new PooledList<HierarchyFacetValue>(Allocator.ArrayPool, capacity: selectedValues.Count + filteredTokens.Count);
#pragma warning restore CA2000 // Dispose objects before losing scope
            var selectedCount = 0;

            for (var i = 0; i < selectedValues.Count; i++)
            {
                values.Add(default);
            }

            foreach (var selectedToken in selectedValues)
            {
                if (this.tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    var parent = this.tokenEncoder.Decode(parentId);
                    tokenCounts.TryGetValue(parentId, out var parentCount);

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        var offset = values.Count;
                        var count = 0;

                        foreach (var childId in children)
                        {
                            if (tokenCounts.TryGetValue(childId, out var childCount))
                            {
                                var child = this.tokenEncoder.Decode(childId);

                                values.Add(new HierarchyFacetValue(child, childCount));
                                count++;
                            }
                        }

                        values.Sort(offset, count, HierarchyFacetValueComparer.Instance);

                        values[selectedCount++] = new HierarchyFacetValue(parent, parentCount, values.Range(offset, count));
                    }
                    else
                    {
                        values[selectedCount++] = new HierarchyFacetValue(parent, parentCount);
                    }
                }
            }

            values.Sort(0, selectedCount, HierarchyFacetValueComparer.Instance);

            return new FieldFacetResult(hierarchyFacetExpression.CreateResult(values.Range(0, selectedCount)), values);
        }

        private sealed class HierarchyFacetValueComparer : IComparer<HierarchyFacetValue>
        {
            private HierarchyFacetValueComparer()
            {
            }

            public static IComparer<HierarchyFacetValue> Instance { get; } = new HierarchyFacetValueComparer();

            public int Compare(HierarchyFacetValue x, HierarchyFacetValue y)
            {
                return y.Count.CompareTo(x.Count);
            }
        }
    }
}
