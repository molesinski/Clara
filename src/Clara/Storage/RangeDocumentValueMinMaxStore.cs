using System;
using System.Collections.Generic;
using Clara.Collections;
using Clara.Querying;

namespace Clara.Storage
{
    internal class RangeDocumentValueMinMaxStore<TValue> : IDisposable
        where TValue : struct, IComparable<TValue>
    {
        private readonly TValue minValue;
        private readonly TValue maxValue;
        private readonly PooledDictionary<int, MinMax<TValue>> documentValueMinMax;

        public RangeDocumentValueMinMaxStore(
            TValue minValue,
            TValue maxValue,
            PooledDictionary<int, MinMax<TValue>> documentValueMinMax)
        {
            if (documentValueMinMax is null)
            {
                throw new ArgumentNullException(nameof(documentValueMinMax));
            }

            this.minValue = minValue;
            this.maxValue = maxValue;
            this.documentValueMinMax = documentValueMinMax;
        }

        public FacetResult? Facet(RangeFacetExpression<TValue> rangeFacetExpression, IEnumerable<int> documents)
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
                return rangeFacetExpression.CreateResult(min, max);
            }

            return null;
        }

        public void Sort(SortDirection direction, DocumentSort documentSort)
        {
            if (direction == SortDirection.Descending)
            {
                var minValue = this.minValue;

                documentSort.Sort(o => this.documentValueMinMax.TryGetValue(o, out var maxMax) ? maxMax.Max : minValue, direction);
            }
            else
            {
                var maxValue = this.maxValue;

                documentSort.Sort(o => this.documentValueMinMax.TryGetValue(o, out var maxMax) ? maxMax.Min : maxValue, direction);
            }
        }

        public void Dispose()
        {
            this.documentValueMinMax.Dispose();
        }
    }
}
