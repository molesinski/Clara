using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly DocumentResultCollection<TDocument> documents;
        private readonly FacetResultCollection facets;
        private bool isDisposed;

        internal QueryResult(
            TokenEncoder tokenEncoder,
            DictionarySlim<int, TDocument> documentMap,
            DocumentScoring documentScoring,
            DocumentList documentList,
            ObjectPoolLease<ListSlim<FacetResult>> facetResults)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documentMap is null)
            {
                throw new ArgumentNullException(nameof(documentMap));
            }

            this.documents = new DocumentResultCollection<TDocument>(tokenEncoder, documentMap, documentScoring, documentList);
            this.facets = new FacetResultCollection(facetResults);
        }

        public DocumentResultCollection<TDocument> Documents
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.documents;
            }
        }

        public FacetResultCollection Facets
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.facets;
            }
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.documents.Dispose();
                this.facets.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
