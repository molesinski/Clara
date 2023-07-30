using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class RangeSortedDocumentValueStore<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private readonly ListSlim<DocumentValue<TValue>> sortedDocumentValues;

        public RangeSortedDocumentValueStore(
            ListSlim<DocumentValue<TValue>> sortedDocumentValues)
        {
            if (sortedDocumentValues is null)
            {
                throw new ArgumentNullException(nameof(sortedDocumentValues));
            }

            sortedDocumentValues.Sort(default(DocumentValueComparer));

            this.sortedDocumentValues = sortedDocumentValues;
        }

        public double FilterOrder
        {
            get
            {
                return this.sortedDocumentValues.Count;
            }
        }

        public void Filter(Field field, TValue? from, TValue? to, DocumentSet documentSet)
        {
            var rangeMatches = new DocumentValueRange<TValue>(this.sortedDocumentValues, from, to);

            documentSet.IntersectWith(field, rangeMatches);
        }

        private readonly struct DocumentValueComparer : IComparer<DocumentValue<TValue>>
        {
            public int Compare(DocumentValue<TValue> x, DocumentValue<TValue> y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }
    }
}
