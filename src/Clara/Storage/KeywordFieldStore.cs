﻿using Clara.Querying;

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

        public override void Filter(FilterExpression filterExpression, DocumentSet documentSet)
        {
            if (filterExpression is KeywordFilterExpression keywordFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    this.tokenDocumentStore.Filter(keywordFilterExpression.Field, keywordFilterExpression.ValuesExpression, documentSet);
                    return;
                }
            }

            base.Filter(filterExpression, documentSet);
        }

        public override FieldFacetResult? Facet(FacetExpression facetExpression, FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            if (facetExpression is KeywordFacetExpression tokenFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return this.documentTokenStore.Facet(tokenFacetExpression, filterExpression, documents);
                }
            }

            return base.Facet(facetExpression, filterExpression, documents);
        }
    }
}
