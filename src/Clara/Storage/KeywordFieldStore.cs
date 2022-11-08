using System;
using System.Collections.Generic;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStore : FieldStore
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly TokenDocumentStore? tokenDocumentStore;
        private readonly KeywordDocumentTokenStore? documentTokenStore;
        private bool isDisposed;

        public KeywordFieldStore(
            ITokenEncoder tokenEncoder,
            TokenDocumentStore? tokenDocumentStore,
            KeywordDocumentTokenStore? documentTokenStore)
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

            if (filterExpression is KeywordFilterExpression keywordFilterExpression)
            {
                if (this.tokenDocumentStore is not null)
                {
                    this.tokenDocumentStore.Filter(keywordFilterExpression.Field, keywordFilterExpression.MatchExpression, documentSet);
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

            if (facetExpression is KeywordFacetExpression tokenFacetExpression)
            {
                if (this.documentTokenStore is not null)
                {
                    return this.documentTokenStore.Facet(tokenFacetExpression, filterExpression, documents);
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
