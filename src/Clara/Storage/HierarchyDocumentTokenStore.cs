using System;
using System.Collections.Generic;
using Clara.Collections;
using Clara.Querying;

namespace Clara.Storage
{
    internal class HierarchyDocumentTokenStore : IDisposable
    {
        private readonly string root;
        private readonly TokenEncoder tokenEncoder;
        private readonly PooledDictionary<int, PooledSet<int>> documentTokens;
        private readonly PooledDictionary<int, PooledSet<int>> parentChildren;

        public HierarchyDocumentTokenStore(
            string root,
            TokenEncoder tokenEncoder,
            PooledDictionary<int, PooledSet<int>> documentTokens,
            PooledDictionary<int, PooledSet<int>> parentChildren)
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

        public FacetResult? Facet(HierarchyFacetExpression hierarchyFacetExpression, IEnumerable<FilterExpression> filterExpressions, IEnumerable<int> documents)
        {
            using var selectedValues = new PooledSet<string>();

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

            var tokenEncoder = this.tokenEncoder;
            using var filteredTokens = new PooledSet<int>();

            foreach (var selectedToken in selectedValues)
            {
                if (tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    filteredTokens.Add(parentId);

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        filteredTokens.UnionWith(children);
                    }
                }
            }

            using var tokenCounts = new PooledDictionary<int, int>();

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

            var values = new List<HierarchyValue>();

            foreach (var selectedToken in selectedValues)
            {
                if (tokenEncoder.TryEncode(selectedToken, out var parentId))
                {
                    var childrenResult = new List<HierarchyValue>();

                    if (this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        foreach (var childId in children)
                        {
                            if (tokenCounts.TryGetValue(childId, out var childCount))
                            {
                                var child = tokenEncoder.Decode(childId);

                                childrenResult.Add(new HierarchyValue(child, childCount));
                            }
                        }

                        childrenResult.Sort(hierarchyFacetExpression.Comparer);
                    }

                    var parent = tokenEncoder.Decode(parentId);
                    tokenCounts.TryGetValue(parentId, out var parentCount);

                    values.Add(new HierarchyValue(parent, parentCount, childrenResult));
                }
            }

            values.Sort(hierarchyFacetExpression.Comparer);

            return hierarchyFacetExpression.CreateResult(values);
        }

        public void Dispose()
        {
            this.documentTokens.Dispose();
            this.parentChildren.Dispose();
        }
    }
}
