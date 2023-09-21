using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class KeywordDocumentTokenStore
    {
        private readonly KeywordField field;
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> documentTokens;

        public KeywordDocumentTokenStore(
            KeywordField field,
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, HashSetSlim<int>> documentTokens)
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

            this.field = field;
            this.tokenEncoder = tokenEncoder;
            this.documentTokens = documentTokens;
        }

        public FacetResult? Facet(FilterExpression? filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            using var selectedValues = SharedObjectPools.SelectedValues.Lease();

            if (filterExpression is TokenFilterExpression tokenFilterExpression)
            {
                if (tokenFilterExpression.ValuesExpression.Values.Count > 0)
                {
                    selectedValues.Instance.UnionWith(tokenFilterExpression.ValuesExpression.Values);
                }
            }

            using var tokenCounts = SharedObjectPools.TokenCounts.Lease();

            foreach (var documentId in documentResultBuilder.GetFacetDocuments(this.field))
            {
                if (this.documentTokens.TryGetValue(documentId, out var tokenIds))
                {
                    foreach (var tokenId in tokenIds)
                    {
                        ref var count = ref tokenCounts.Instance.GetValueRefOrAddDefault(tokenId, out _);

                        count++;
                    }
                }
            }

            var values = SharedObjectPools.KeywordFacetValues.Lease();

            foreach (var pair in tokenCounts.Instance)
            {
                var tokenId = pair.Key;
                var count = pair.Value;
                var token = this.tokenEncoder.Decode(tokenId);
                var isSelected = selectedValues.Instance.Contains(token);

                values.Instance.Add(new KeywordFacetValue(token, count, isSelected));
                selectedValues.Instance.Remove(token);
            }

            foreach (var token in selectedValues.Instance)
            {
                values.Instance.Add(new KeywordFacetValue(token, count: 0, isSelected: true));
            }

            values.Instance.Sort(KeywordFacetValueComparer.Instance);

            return new KeywordFacetResult(this.field, values.Instance, values);
        }

        private sealed class KeywordFacetValueComparer : IComparer<KeywordFacetValue>
        {
            private KeywordFacetValueComparer()
            {
            }

            public static KeywordFacetValueComparer Instance { get; } = new KeywordFacetValueComparer();

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
