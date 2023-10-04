using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class RangeFieldStore<TValue> : FieldStore
        where TValue : struct, IComparable<TValue>
    {
        private readonly RangeSortedDocumentValueStore<TValue>? sortedDocumentValueStore;
        private readonly RangeDocumentValueMinMaxStore<TValue>? documentValueMinMaxStore;

        public RangeFieldStore(
            RangeSortedDocumentValueStore<TValue>? sortedDocumentValueStore,
            RangeDocumentValueMinMaxStore<TValue>? documentValueMinMaxStore)
        {
            this.sortedDocumentValueStore = sortedDocumentValueStore;
            this.documentValueMinMaxStore = documentValueMinMaxStore;
        }

        public override double FilterOrder
        {
            get
            {
                if (this.sortedDocumentValueStore is not null)
                {
                    return this.sortedDocumentValueStore.FilterOrder;
                }

                return base.FilterOrder;
            }
        }

        public override void Filter(FilterExpression filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (filterExpression is RangeFilterExpression<TValue> rangeFilterExpression)
            {
                if (this.sortedDocumentValueStore is not null)
                {
                    this.sortedDocumentValueStore.Filter(rangeFilterExpression.Field, rangeFilterExpression.From, rangeFilterExpression.To, ref documentResultBuilder);
                    return;
                }
            }

            base.Filter(filterExpression, ref documentResultBuilder);
        }

        public override FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (facetExpression is RangeFacetExpression<TValue>)
            {
                if (this.documentValueMinMaxStore is not null)
                {
                    return this.documentValueMinMaxStore.Facet(ref documentResultBuilder);
                }
            }

            return base.Facet(facetExpression, filterExpression, ref documentResultBuilder);
        }

        public override DocumentList Sort(SortExpression sortExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (sortExpression is RangeSortExpression<TValue> rangeSortExpression)
            {
                if (this.documentValueMinMaxStore is not null)
                {
                    return this.documentValueMinMaxStore.Sort(rangeSortExpression.SortDirection, ref documentResultBuilder);
                }
            }

            return base.Sort(sortExpression, ref documentResultBuilder);
        }
    }
}
