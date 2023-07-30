using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class RangeDocumentValueMinMaxStore<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private readonly TValue minValue;
        private readonly TValue maxValue;
        private readonly DictionarySlim<int, MinMax<TValue>> documentValueMinMax;

        public RangeDocumentValueMinMaxStore(
            TValue minValue,
            TValue maxValue,
            DictionarySlim<int, MinMax<TValue>> documentValueMinMax)
        {
            if (documentValueMinMax is null)
            {
                throw new ArgumentNullException(nameof(documentValueMinMax));
            }

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.documentValueMinMax = documentValueMinMax;
        }

        public FieldFacetResult? Facet(RangeFacetExpression<TValue> rangeFacetExpression, IEnumerable<int> documents)
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
                return new FieldFacetResult(rangeFacetExpression.CreateResult(min, max));
            }

            return null;
        }

        public SortedDocumentSet Sort(SortDirection direction, DocumentSet documentSet)
        {
            if (direction == SortDirection.Descending)
            {
                var documentValueMinMax = this.documentValueMinMax;
                var minValue = this.minValue;

                return new RangeSortedDocumentSet<TValue>(
                    o =>
                    {
                        if (documentValueMinMax.TryGetValue(o, out var maxMax))
                        {
                            return maxMax.Max;
                        }

                        return minValue;
                    },
                    DocumentValueComparer<TValue>.Descending,
                    documentSet);
            }
            else
            {
                var documentValueMinMax = this.documentValueMinMax;
                var maxValue = this.maxValue;

                return new RangeSortedDocumentSet<TValue>(
                    o =>
                    {
                        if (documentValueMinMax.TryGetValue(o, out var maxMax))
                        {
                            return maxMax.Min;
                        }

                        return maxValue;
                    },
                    DocumentValueComparer<TValue>.Ascending,
                    documentSet);
            }
        }
    }
}
