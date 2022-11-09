using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStore : FieldStore
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly TokenDocumentStore? tokenDocumentStore;
        private readonly HierarchyDocumentTokenStore? documentTokenStore;
        private bool isDisposed;

        public HierarchyFieldStore(
            ITokenEncoder tokenEncoder,
            TokenDocumentStore? tokenDocumentStore,
            HierarchyDocumentTokenStore? documentTokenStore)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            this.tokenEncoder = tokenEncoder;
            this.tokenDocumentStore = tokenDocumentStore;
            this.documentTokenStore = documentTokenStore;
        }

        public override double FilterOrder
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new InvalidOperationException("Current instance is already disposed.");
                }

                if (this.tokenDocumentStore is not null)
                {
                    return this.tokenDocumentStore.FilterOrder;
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

            if (filterExpression is HierarchyFilterExpression hierarchyFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    this.tokenDocumentStore.Filter(hierarchyFilterExpression.Field, hierarchyFilterExpression.MatchExpression, documentSet);
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

            if (facetExpression is HierarchyFacetExpression hierarchyFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return this.documentTokenStore.Facet(hierarchyFacetExpression, filterExpression, documents);
                }
            }

            return base.Facet(facetExpression, filterExpression, documents);
        }

        public override void Dispose()
        {
            if (!this.isDisposed)
            {
                this.tokenEncoder.Dispose();
                this.tokenDocumentStore?.Dispose();
                this.documentTokenStore?.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
