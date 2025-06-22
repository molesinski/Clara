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

            documentValues.Sort(DocumentValueComparer<TValue>.Ascending);

            this.sortedDocumentValues = documentValues;
            this.FilterOrder = documentValues.Count;
        }

        public double FilterOrder { get; }

        public DocumentSet Filter(TValue? valueFrom, TValue? valueTo)
        {
            var list = this.sortedDocumentValues;

            var lo = 0;
            var hi = list.Count - 1;

            if (valueFrom is not null)
            {
                lo = BinarySearchLow(list, valueFrom.Value);
            }

            if (valueTo is not null)
            {
                hi = BinarySearchHigh(list, valueTo.Value);
            }

            var documents = SharedObjectPools.DocumentSets.Lease();

            for (var i = lo; i <= hi; i++)
            {
                documents.Instance.Add(list[i].DocumentId);
            }

            return new DocumentSet(documents);
        }

        private static int BinarySearchLow(ListSlim<DocumentValue<TValue>> list, TValue value)
        {
            var left = 0;
            var right = list.Count - 1;

            while (left <= right)
            {
                var mid = (left + right) / 2;

                if (list[mid].Value.CompareTo(value) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return left;
        }

        private static int BinarySearchHigh(ListSlim<DocumentValue<TValue>> list, TValue value)
        {
            var left = 0;
            var right = list.Count - 1;

            while (left <= right)
            {
                var mid = (left + right) / 2;

                if (list[mid].Value.CompareTo(value) <= 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return right;
        }
    }
}
