using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class KeywordDocumentTokenStore
    {
        private readonly KeywordField field;
        private readonly TokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> documentTokens;

        public KeywordDocumentTokenStore(
            KeywordField field,
            TokenEncoder tokenEncoder,
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

            using var tokenCounts = SharedObjectPools.TokenCounts.Lease();

            foreach (var documentId in documents)
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

            return new KeywordFacetResult(this.field, values);
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
