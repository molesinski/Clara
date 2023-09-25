using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyDocumentTokenStore
    {
        private readonly string root;
        private readonly HierarchyField field;
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> documentTokens;
        private readonly DictionarySlim<int, HashSetSlim<int>> parentChildren;

        public HierarchyDocumentTokenStore(
            HierarchyField field,
            string root,
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, HashSetSlim<int>> documentTokens,
            DictionarySlim<int, HashSetSlim<int>> parentChildren)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

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

            this.root = root;
            this.field = field;
            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
            this.parentChildren = parentChildren;
        }

        public FacetResult Facet(HierarchyFilterExpression? hierarchyFilterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            using var selectedValues = SharedObjectPools.ValueSets.Lease();

            if (hierarchyFilterExpression is not null)
            {
                if (hierarchyFilterExpression.ValuesExpression.Values.Count > 0)
                {
                    selectedValues.Instance.UnionWith(hierarchyFilterExpression.ValuesExpression.Values);
                }
            }

            if (selectedValues.Instance.Count == 0)
            {
                selectedValues.Instance.Add(this.root);
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

            foreach (var documentId in documentResultBuilder.GetFacetDocuments(this.field))
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

                        values.Instance[selectedCount++] = new HierarchyFacetValue(parent, parentCount, values.Instance.Range(offset, count));
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
