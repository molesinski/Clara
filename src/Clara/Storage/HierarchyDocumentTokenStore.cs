using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Clara.Querying;

namespace Clara.Storage
{
    internal class HierarchyDocumentTokenStore
    {
        private readonly string root;
        private readonly TokenEncoder tokenEncoder;
        private readonly Dictionary<int, HashSet<int>> documentTokens;
        private readonly Dictionary<int, HashSet<int>> parentChildren;

        public HierarchyDocumentTokenStore(
            string root,
            TokenEncoder tokenEncoder,
            Dictionary<int, HashSet<int>> documentTokens,
            Dictionary<int, HashSet<int>> parentChildren)
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

            var tokenEncoder = this.tokenEncoder;
            var filteredTokens = new HashSet<int>();

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

            var tokenCounts = new Dictionary<int, int>();

            foreach (var documentId in documents)
            {
                if (this.documentTokens.TryGetValue(documentId, out var tokenIds))
                {
                    foreach (var tokenId in tokenIds)
                    {
                        if (filteredTokens.Contains(tokenId))
                        {
#if NET6_0_OR_GREATER
                            ref var count = ref CollectionsMarshal.GetValueRefOrAddDefault(tokenCounts, tokenId, out _);

                            count++;
#else
                            if (tokenCounts.TryGetValue(tokenId, out var count))
                            {
                                tokenCounts[tokenId] = count++;
                            }
                            else
                            {
                                tokenCounts.Add(tokenId, 1);
                            }
#endif
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
    }
}
