using System;
using System.Collections.Generic;
using Clara.Collections;
using Clara.Querying;

namespace Clara.Storage
{
    internal class KeywordDocumentTokenStore : IDisposable
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly PooledDictionary<int, PooledSet<int>> documentTokens;

        public KeywordDocumentTokenStore(
            TokenEncoder tokenEncoder,
            PooledDictionary<int, PooledSet<int>> documentTokens)
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

            using var tokenCounts = new PooledDictionary<int, int>();

            foreach (var documentId in documents)
            {
                if (this.documentTokens.TryGetValue(documentId, out var tokenIds))
                {
                    foreach (var tokenId in tokenIds)
                    {
                        ref var count = ref tokenCounts.GetValueRefOrAddDefault(tokenId, out _);

                        count++;
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

        public void Dispose()
        {
            this.documentTokens.Dispose();
        }
    }
}
