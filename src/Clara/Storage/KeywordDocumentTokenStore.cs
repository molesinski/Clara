using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class KeywordDocumentTokenStore
    {
        private static readonly HashSet<string> EmptySelectedValues = new();
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

        public FacetResult? Facet(FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            var selectedValues = EmptySelectedValues;

            if (filterExpression is TokenFilterExpression tokenFilterExpression)
            {
                if (tokenFilterExpression.ValuesExpression.Values.Count > 0)
                {
                    selectedValues = new HashSet<string>(tokenFilterExpression.ValuesExpression.Values);
                }
            }

            using var tokenCounts = DictionarySlim<int, int>.ObjectPool.Lease();

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

            var values = ListSlim<KeywordFacetValue>.ObjectPool.Lease();

            foreach (var pair in tokenCounts.Instance)
            {
                var tokenId = pair.Key;
                var count = pair.Value;
                var token = this.tokenEncoder.Decode(tokenId);
                var isSelected = selectedValues.Contains(token);

                values.Instance.Add(new KeywordFacetValue(token, count, isSelected));
                selectedValues.Remove(token);
            }

            foreach (var token in selectedValues)
            {
                values.Instance.Add(new KeywordFacetValue(token, count: 0, isSelected: true));
            }

            values.Instance.Sort(KeywordFacetValueComparer.Instance);

            return new KeywordFacetResult(this.field, values.Instance, values);
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
