using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class RangeDocumentValueMinMaxStore<TValue>
        where TValue : struct, IComparable<TValue>
    {
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

        public FacetResult? Facet(HashSetSlim<int> documents)
        {
            var hasMinMax = false;
            var min = this.maxValue;
            var max = this.minValue;

            foreach (var documentId in documents)
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

        public SortedDocumentSet Sort(SortDirection direction, DocumentSet documentSet)
        {
            if (direction == SortDirection.Descending)
            {
                var documentValueMinMax = this.documentValueMinMax;
                var minValue = this.minValue;

                return
                    new SortedDocumentSet<TValue>(
                        documentSet,
                        o =>
                        {
                            if (documentValueMinMax.TryGetValue(o, out var maxMax))
                            {
                                return maxMax.Max;
                            }

                            return minValue;
                        },
                        DocumentValueComparer<TValue>.Descending);
            }
            else
            {
                var documentValueMinMax = this.documentValueMinMax;
                var maxValue = this.maxValue;

                return
                    new SortedDocumentSet<TValue>(
                        documentSet,
                        o =>
                        {
                            if (documentValueMinMax.TryGetValue(o, out var maxMax))
                            {
                                return maxMax.Min;
                            }

                            return maxValue;
                        },
                        DocumentValueComparer<TValue>.Ascending);
            }
        }
    }
}
