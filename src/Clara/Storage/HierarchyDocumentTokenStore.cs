﻿using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyDocumentTokenStore
    {
        private readonly HashSet<string> rootSet;
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

            this.rootSet = new HashSet<string> { root };
            this.field = field;
            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
            this.parentChildren = parentChildren;
        }

        public FacetResult Facet(FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            var selectedValues = this.rootSet;

            if (filterExpression is TokenFilterExpression tokenFilterExpression)
            {
                if (tokenFilterExpression.ValuesExpression.Values.Count > 0)
                {
                    selectedValues = new HashSet<string>(tokenFilterExpression.ValuesExpression.Values);
                }
            }

            using var filteredTokens = HashSetSlim<int>.ObjectPool.Lease();

            foreach (var selectedToken in selectedValues)
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

            using var tokenCounts = DictionarySlim<int, int>.ObjectPool.Lease();

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

            var values = ListSlim<HierarchyFacetValue>.ObjectPool.Lease();
            var selectedCount = 0;

            for (var i = 0; i < selectedValues.Count; i++)
            {
                values.Instance.Add(default);
            }

            foreach (var selectedToken in selectedValues)
            {
                if (this.tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    var parent = this.tokenEncoder.Decode(parentId);
                    var parentCount = tokenCounts.Instance[parentId];

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

            return new HierarchyFacetResult(this.field, values.Instance.Range(0, selectedCount), values);
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
