using Clara.Querying;
using Clara.Utils;

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

        public override double? FilterOrder
        {
            get
            {
                return this.sortedDocumentValueStore?.FilterOrder;
            }
        }

        public override DocumentSet Filter(FilterExpression filterExpression)
        {
            if (filterExpression is RangeFilterExpression<TValue> rangeFilterExpression)
            {
                if (this.sortedDocumentValueStore is not null)
                {
                    return
                        this.sortedDocumentValueStore.Filter(
                            rangeFilterExpression.ValueFrom,
                            rangeFilterExpression.ValueTo);
                }
            }

            return base.Filter(filterExpression);
        }

        public override FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, HashSetSlim<int> documents)
        {
            if (facetExpression is RangeFacetExpression<TValue>)
            {
                if (this.documentValueMinMaxStore is not null)
                {
                    return
                        this.documentValueMinMaxStore.Facet(
                            documents);
                }
            }

            return base.Facet(facetExpression, filterExpression, documents);
        }

        public override DocumentList Sort(SortExpression sortExpression, HashSetSlim<int> documents)
        {
            if (sortExpression is RangeSortExpression<TValue> rangeSortExpression)
            {
                if (this.documentValueMinMaxStore is not null)
                {
                    return
                        this.documentValueMinMaxStore.Sort(
                            rangeSortExpression.SortDirection,
                            documents);
                }
            }

            return base.Sort(sortExpression, documents);
        }
    }
}
