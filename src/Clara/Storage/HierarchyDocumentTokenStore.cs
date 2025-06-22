using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyDocumentTokenStore
    {
        private readonly HierarchyField field;
        private readonly TokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> documentTokens;
        private readonly DictionarySlim<int, HashSetSlim<int>> parentChildren;

        public HierarchyDocumentTokenStore(
            HierarchyField field,
            TokenEncoder tokenEncoder,
            DictionarySlim<int, HashSetSlim<int>> documentTokens,
            DictionarySlim<int, HashSetSlim<int>> parentChildren)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
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

            this.field = field;
            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
            this.parentChildren = parentChildren;
        }

        public FacetResult Facet(FilterValueCollection? filterValues, HashSetSlim<int> documents)
        {
            using var selectedValues = SharedObjectPools.FilterValues.Lease();

            if (filterValues?.Count > 0)
            {
                foreach (var item in filterValues)
                {
                    selectedValues.Instance.Add(item);
                }
            }

            if (selectedValues.Instance.Count == 0)
            {
                selectedValues.Instance.Add(this.field.Root);
            }

            using var filteredTokens = SharedObjectPools.FilteredTokens.Lease();

            foreach (var selectedToken in selectedValues.Instance)
            {
                if (this.tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    filteredTokens.Instance.Add(parentId);

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        filteredTokens.Instance.UnionWith(children);
                    }
                }
            }

            using var tokenCounts = SharedObjectPools.TokenCounts.Lease();

            foreach (var documentId in documents)
            {
                if (this.documentTokens.TryGetValue(documentId, out var tokenIds))
                {
                    foreach (var tokenId in tokenIds)
                    {
                        if (filteredTokens.Instance.Contains(tokenId))
                        {
                            ref var count = ref tokenCounts.Instance.GetValueRefOrAddDefault(tokenId, out _);

                            count++;
                        }
                    }
                }
            }

            var values = SharedObjectPools.HierarchyFacetValues.Lease();
            var selectedCount = 0;

            for (var i = 0; i < selectedValues.Instance.Count; i++)
            {
                values.Instance.Add(default);
            }

            foreach (var selectedToken in selectedValues.Instance)
            {
                if (this.tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    var parent = this.tokenEncoder.Decode(parentId);
                    tokenCounts.Instance.TryGetValue(parentId, out var parentCount);

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        var offset = values.Instance.Count;
                        var count = 0;

                        foreach (var childId in children)
                        {
                            if (tokenCounts.Instance.TryGetValue(childId, out var childCount))
                            {
                                var child = this.tokenEncoder.Decode(childId);

                                values.Instance.Add(new HierarchyFacetValue(child, childCount));
                                count++;
                            }
                        }

                        values.Instance.Sort(offset, count, HierarchyFacetValueComparer.Instance);

                        if (count > 0)
                        {
                            values.Instance[selectedCount++] = new HierarchyFacetValue(parent, parentCount, new HierarchyFacetValueChildrenCollection(values.Instance, offset, count));
                        }
                        else
                        {
                            values.Instance[selectedCount++] = new HierarchyFacetValue(parent, parentCount);
                        }
                    }
                    else
                    {
                        values.Instance[selectedCount++] = new HierarchyFacetValue(parent, parentCount);
                    }
                }
            }

            values.Instance.Sort(0, selectedCount, HierarchyFacetValueComparer.Instance);

            return new HierarchyFacetResult(this.field, values, 0, selectedCount);
        }

        private sealed class HierarchyFacetValueComparer : IComparer<HierarchyFacetValue>
        {
            private HierarchyFacetValueComparer()
            {
            }

            public static HierarchyFacetValueComparer Instance { get; } = new HierarchyFacetValueComparer();

            public int Compare(HierarchyFacetValue x, HierarchyFacetValue y)
            {
                return y.Count.CompareTo(x.Count);
            }
        }
    }
}
