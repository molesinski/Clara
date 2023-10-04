using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStore : FieldStore
    {
        private readonly TokenDocumentStore? tokenDocumentStore;
        private readonly HierarchyDocumentTokenStore? documentTokenStore;

        public HierarchyFieldStore(
            TokenDocumentStore? tokenDocumentStore,
            HierarchyDocumentTokenStore? documentTokenStore)
        {
            this.tokenDocumentStore = tokenDocumentStore;
            this.documentTokenStore = documentTokenStore;
        }

        public override double FilterOrder
        {
            get
            {
                if (this.tokenDocumentStore is not null)
                {
                    return this.tokenDocumentStore.FilterOrder;
                }

                return base.FilterOrder;
            }
        }

        public override void Filter(FilterExpression filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (filterExpression is HierarchyFilterExpression hierarchyFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    this.tokenDocumentStore.Filter(hierarchyFilterExpression.Field, hierarchyFilterExpression.FilterMode, (HashSetSlim<string>)hierarchyFilterExpression.Values, ref documentResultBuilder);
                    return;
                }
            }

            base.Filter(filterExpression, ref documentResultBuilder);
        }

        public override FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (facetExpression is HierarchyFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return this.documentTokenStore.Facet(filterExpression as HierarchyFilterExpression, ref documentResultBuilder);
                }
            }

            return base.Facet(facetExpression, filterExpression, ref documentResultBuilder);
        }
    }
}
