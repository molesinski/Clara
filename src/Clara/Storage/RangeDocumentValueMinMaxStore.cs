using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class RangeDocumentValueMinMaxStore<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private static readonly ObjectPool<ListSlim<DocumentValue<TValue>>> SortedDocumentListsPool = new(() => new());

        private readonly RangeField<TValue> field;
        private readonly TValue minValue;
        private readonly TValue maxValue;
        private readonly DictionarySlim<int, MinMax<TValue>> documentValueMinMax;

        public RangeDocumentValueMinMaxStore(
            RangeField<TValue> field,
            TValue minValue,
            TValue maxValue,
            DictionarySlim<int, MinMax<TValue>> documentValueMinMax)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (documentValueMinMax is null)
            {
                throw new ArgumentNullException(nameof(documentValueMinMax));
            }

            this.field = field;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.documentValueMinMax = documentValueMinMax;
        }

        public FacetResult? Facet(ref DocumentResultBuilder documentResultBuilder)
        {
            var hasMinMax = false;
            var min = this.maxValue;
            var max = this.minValue;

            foreach (var documentId in documentResultBuilder.GetFacetDocuments(this.field))
            {
                if (this.documentValueMinMax.TryGetValue(documentId, out var minMax))
                {
                    if (minMax.Min.CompareTo(min) < 0)
                    {
                        min = minMax.Min;
                    }

                    if (minMax.Max.CompareTo(max) > 0)
                    {
                        max = minMax.Max;
                    }

                    hasMinMax = true;
                }
            }

            if (hasMinMax)
            {
                return new RangeFacetResult<TValue>(this.field, min, max);
            }

            return null;
        }

        public DocumentList Sort(SortDirection direction, ref DocumentResultBuilder documentResultBuilder)
        {
            if (direction == SortDirection.Descending)
            {
                return
                    Sort(
                        documentResultBuilder.Documents,
                        this.GetDescendingValue,
                        DocumentValueComparer<TValue>.Descending);
            }
            else
            {
                return
                    Sort(
                        documentResultBuilder.Documents,
                        this.GetAscendingValue,
                        DocumentValueComparer<TValue>.Ascending);
            }
        }

        private static DocumentList Sort(
            HashSetSlim<int> documentSet,
            Func<int, TValue> valueSelector,
            IComparer<DocumentValue<TValue>> comparer)
        {
            using var sortedDocuments = SortedDocumentListsPool.Lease();

            foreach (var documentId in documentSet)
            {
                var value = valueSelector(documentId);

                sortedDocuments.Instance.Add(new DocumentValue<TValue>(documentId, value));
            }

            sortedDocuments.Instance.Sort(comparer);

            var documents = SharedObjectPools.Documents.Lease();

            foreach (var documentValue in sortedDocuments.Instance)
            {
                documents.Instance.Add(documentValue.DocumentId);
            }

            return new DocumentList(documents);
        }

        private TValue GetDescendingValue(int documentId)
        {
            if (this.documentValueMinMax.TryGetValue(documentId, out var minMax))
            {
                return minMax.Max;
            }

            return this.minValue;
        }

        private TValue GetAscendingValue(int documentId)
        {
            if (this.documentValueMinMax.TryGetValue(documentId, out var minMax))
            {
                return minMax.Min;
            }

            return this.maxValue;
        }
    }
}
