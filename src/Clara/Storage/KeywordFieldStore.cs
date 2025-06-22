using Clara.Querying;
using Clara.Utils;

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

        public override double? FilterOrder
        {
            get
            {
                return this.tokenDocumentStore?.FilterOrder;
            }
        }

        public override DocumentSet Filter(FilterExpression filterExpression)
        {
            if (filterExpression is KeywordFilterExpression keywordFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    return
                        this.tokenDocumentStore.Filter(
                            keywordFilterExpression.FilterMode,
                            keywordFilterExpression.Values);
                }
            }

            return base.Filter(filterExpression);
        }

        public override FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, HashSetSlim<int> documents)
        {
            if (facetExpression is KeywordFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return
                        this.documentTokenStore.Facet(
                            (filterExpression as KeywordFilterExpression)?.Values,
                            documents);
                }
            }

            return base.Facet(facetExpression, filterExpression, documents);
        }
    }
}
