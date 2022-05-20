using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Clara.Querying;

namespace Clara.Storage
{
    internal class KeywordDocumentTokenStore
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly Dictionary<int, HashSet<int>> documentTokens;

        public KeywordDocumentTokenStore(
            TokenEncoder tokenEncoder,
            Dictionary<int, HashSet<int>> documentTokens)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documentTokens is null)
            {
                throw new ArgumentNullException(nameof(documentTokens));
            }

            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
        }

        public FacetResult? Facet(KeywordFacetExpression tokenFacetExpression, IEnumerable<FilterExpression> filterExpressions, IEnumerable<int> documents)
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

            var tokenCounts = new Dictionary<int, int>();

            foreach (var documentId in documents)
            {
                if (this.documentTokens.TryGetValue(documentId, out var tokenIds))
                {
                    foreach (var tokenId in tokenIds)
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

            var values = new List<KeywordValue>();

            foreach (var pair in tokenCounts)
            {
                var tokenId = pair.Key;
                var count = pair.Value;
                var token = this.tokenEncoder.Decode(tokenId);
                var isSelected = selectedValues.Contains(token);

                values.Add(new KeywordValue(token, count, isSelected));
                selectedValues.Remove(token);
            }

            foreach (var token in selectedValues)
            {
                values.Add(new KeywordValue(token, count: 0, isSelected: true));
            }

            values.Sort(tokenFacetExpression.Comparer);

            return tokenFacetExpression.CreateResult(values);
        }
    }
}
