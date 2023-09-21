using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class RangeSortedDocumentValueStore<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private readonly ListSlim<DocumentValue<TValue>> sortedDocumentValues;

        public RangeSortedDocumentValueStore(
            ListSlim<DocumentValue<TValue>> documentValues)
        {
            if (documentValues is null)
            {
                throw new ArgumentNullException(nameof(documentValues));
            }

            documentValues.Sort(DocumentValueComparer.Instance);

            this.sortedDocumentValues = documentValues;
        }

        public double FilterOrder
        {
            get
            {
                return this.sortedDocumentValues.Count;
            }
        }

        public void Filter(Field field, TValue? from, TValue? to, ref DocumentResultBuilder documentResultBuilder)
        {
            var rangeMatches = new DocumentValueRange<TValue>(this.sortedDocumentValues, from, to);

            documentResultBuilder.IntersectWith(field, rangeMatches);
        }

        private sealed class DocumentValueComparer : IComparer<DocumentValue<TValue>>
        {
            private DocumentValueComparer()
            {
            }

            public static DocumentValueComparer Instance { get; } = new DocumentValueComparer();

            public int Compare(DocumentValue<TValue> x, DocumentValue<TValue> y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }
    }
}
