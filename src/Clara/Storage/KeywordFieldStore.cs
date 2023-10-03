using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStore : FieldStore
    {
        private readonly TokenDocumentStore? tokenDocumentStore;
        private readonly KeywordDocumentTokenStore? documentTokenStore;

        public KeywordFieldStore(
            TokenDocumentStore? tokenDocumentStore,
            KeywordDocumentTokenStore? documentTokenStore)
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
            if (filterExpression is KeywordFilterExpression keywordFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    this.tokenDocumentStore.Filter(keywordFilterExpression.Field, keywordFilterExpression.ValuesExpression, ref documentResultBuilder);
                    return;
                }
            }

            base.Filter(filterExpression, ref documentResultBuilder);
        }

        public override FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (facetExpression is KeywordFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return this.documentTokenStore.Facet(filterExpression as KeywordFilterExpression, ref documentResultBuilder);
                }
            }

            return base.Facet(facetExpression, filterExpression, ref documentResultBuilder);
        }
    }
}
