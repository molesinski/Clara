using System;
using System.Collections.Generic;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal class HierarchyDocumentTokenStore : IDisposable
    {
        private readonly string root;
        private readonly ITokenEncoder tokenEncoder;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>> documentTokens;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>> parentChildren;

        public HierarchyDocumentTokenStore(
            string root,
            ITokenEncoder tokenEncoder,
            PooledDictionarySlim<int, PooledHashSetSlim<int>> documentTokens,
            PooledDictionarySlim<int, PooledHashSetSlim<int>> parentChildren)
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

            this.root = root;
            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
            this.parentChildren = parentChildren;
        }

        public FieldFacetResult? Facet(HierarchyFacetExpression hierarchyFacetExpression, IEnumerable<FilterExpression> filterExpressions, IEnumerable<int> documents)
        {
            var selectedValues = new HashSet<string>();

            foreach (var filterExpression in filterExpressions)
            {
                if (filterExpression is TokenFilterExpression tokenFilterExpression)
                {
                    if (tokenFilterExpression.MatchExpression is ValuesMatchExpression valuesMatchExpression)
                    {
                        selectedValues.UnionWith(valuesMatchExpression.Values);
                    }
                }
            }

            if (selectedValues.Count == 0)
            {
                selectedValues.Add(this.root);
            }

            using var filteredTokens = new PooledHashSetSlim<int>();

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

            using var tokenCounts = new PooledDictionarySlim<int, int>(capacity: filteredTokens.Count);

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

            var values = new List<HierarchyFacetValue>(capacity: selectedValues.Count);
            var childrenValues = new PooledList<HierarchyFacetValue>(capacity: filteredTokens.Count);

            foreach (var selectedToken in selectedValues)
            {
                if (this.tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    var parent = this.tokenEncoder.Decode(parentId);
                    tokenCounts.TryGetValue(parentId, out var parentCount);

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        var offset = childrenValues.Count;
                        var count = 0;

                        foreach (var childId in children)
                        {
                            if (tokenCounts.TryGetValue(childId, out var childCount))
                            {
                                var child = this.tokenEncoder.Decode(childId);

                                childrenValues.Add(new HierarchyFacetValue(child, childCount));
                                count++;
                            }
                        }

                        childrenValues.Sort(offset, count, HierarchyFacetValueComparer.Instance);

                        values.Add(new HierarchyFacetValue(parent, parentCount, childrenValues.Range(offset, count)));
                    }
                    else
                    {
                        values.Add(new HierarchyFacetValue(parent, parentCount));
                    }
                }
            }

            values.Sort(HierarchyFacetValueComparer.Instance);

            return new FieldFacetResult(hierarchyFacetExpression.CreateResult(values), new[] { childrenValues });
        }

        public void Dispose()
        {
            this.documentTokens.Dispose();
            this.parentChildren.Dispose();
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
