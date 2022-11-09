using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class RangeFieldStore<TValue> : FieldStore
        where TValue : struct, IComparable<TValue>
    {
        private readonly RangeSortedDocumentValueStore<TValue>? sortedDocumentValueStore;
        private readonly RangeDocumentValueMinMaxStore<TValue>? documentValueMinMaxStore;
        private bool isDisposed;

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
                if (this.isDisposed)
                {
                    throw new InvalidOperationException("Current instance is already disposed.");
                }

                if (this.sortedDocumentValueStore is not null)
                {
                    return this.sortedDocumentValueStore.FilterOrder;
                }

                return base.FilterOrder;
            }
        }

        public override void Filter(FilterExpression filterExpression, DocumentSet documentSet)
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Current instance is already disposed.");
            }

            if (filterExpression is RangeFilterExpression<TValue> rangeFilterExpression)
            {
                if (this.sortedDocumentValueStore is not null)
                {
                    this.sortedDocumentValueStore.Filter(rangeFilterExpression.Field, rangeFilterExpression.From, rangeFilterExpression.To, documentSet);
                    return;
                }
            }

            base.Filter(filterExpression, documentSet);
        }

        public override FieldFacetResult? Facet(FacetExpression facetExpression, FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Current instance is already disposed.");
            }

            if (facetExpression is RangeFacetExpression<TValue> rangeFacetExpression)
            {
                if (this.documentValueMinMaxStore is not null)
                {
                    return this.documentValueMinMaxStore.Facet(rangeFacetExpression, documents);
                }
            }

            return base.Facet(facetExpression, filterExpression, documents);
        }

        public override SortedDocumentSet Sort(SortExpression sortExpression, DocumentSet documentSet)
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Current instance is already disposed.");
            }

            if (sortExpression is RangeSortExpression<TValue> rangeSortExpression)
            {
                if (this.documentValueMinMaxStore is not null)
                {
                    return this.documentValueMinMaxStore.Sort(rangeSortExpression.Direction, documentSet);
                }
            }

            return base.Sort(sortExpression, documentSet);
        }

        public override void Dispose()
        {
            if (!this.isDisposed)
            {
                this.sortedDocumentValueStore?.Dispose();
                this.documentValueMinMaxStore?.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
