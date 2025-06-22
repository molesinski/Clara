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

        public override double? FilterOrder
        {
            get
            {
                return this.tokenDocumentStore?.FilterOrder;
            }
        }

        public override DocumentSet Filter(FilterExpression filterExpression)
        {
            if (filterExpression is HierarchyFilterExpression hierarchyFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    return
                        this.tokenDocumentStore.Filter(
                            hierarchyFilterExpression.FilterMode,
                            hierarchyFilterExpression.Values);
                }
            }

            return base.Filter(filterExpression);
        }

        public override FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, HashSetSlim<int> documents)
        {
            if (facetExpression is HierarchyFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return
                        this.documentTokenStore.Facet(
                            (filterExpression as HierarchyFilterExpression)?.Values,
                            documents);
                }
            }

            return base.Facet(facetExpression, filterExpression, documents);
        }
    }
}
