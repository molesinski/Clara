using System;
using System.Collections.Generic;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal class KeywordDocumentTokenStore : IDisposable
    {
        private static readonly HashSet<string> EmptySelectedValues = new();

        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> documentTokens;

        public KeywordDocumentTokenStore(
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, HashSetSlim<int>> documentTokens)
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

        public FieldFacetResult? Facet(KeywordFacetExpression tokenFacetExpression, FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            var selectedValues = EmptySelectedValues;

            if (filterExpression is TokenFilterExpression tokenFilterExpression)
            {
                if (tokenFilterExpression.MatchExpression is ValuesMatchExpression valuesMatchExpression)
                {
                    selectedValues = new HashSet<string>(valuesMatchExpression.Values);
                }
            }

            using var tokenCounts = new DictionarySlim<int, int>(Allocator.ArrayPool);

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

            var values = new ListSlim<KeywordFacetValue>(Allocator.ArrayPool);

            foreach (var pair in tokenCounts)
            {
                var tokenId = pair.Key;
                var count = pair.Value;
                var token = this.tokenEncoder.Decode(tokenId);
                var isSelected = selectedValues.Contains(token);

                values.Add(new KeywordFacetValue(token, count, isSelected));
                selectedValues.Remove(token);
            }

            foreach (var token in selectedValues)
            {
                values.Add(new KeywordFacetValue(token, count: 0, isSelected: true));
            }

            values.Sort(KeywordFacetValueComparer.Instance);

            return new FieldFacetResult(tokenFacetExpression.CreateResult(values), values);
        }

        public void Dispose()
        {
            foreach (var pair in this.documentTokens)
            {
                pair.Value.Dispose();
            }

            this.documentTokens.Dispose();
        }

        public sealed class KeywordFacetValueComparer : IComparer<KeywordFacetValue>
        {
            private KeywordFacetValueComparer()
            {
            }

            public static IComparer<KeywordFacetValue> Instance { get; } = new KeywordFacetValueComparer();

            public int Compare(KeywordFacetValue x, KeywordFacetValue y)
            {
                var result = y.IsSelected.CompareTo(x.IsSelected);

                if (result != 0)
                {
                    return result;
                }

                return y.Count.CompareTo(x.Count);
            }
        }
    }
}
