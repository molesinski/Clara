using Clara.Utils;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly DocumentResultCollection<TDocument> documentResults;
        private readonly ObjectPoolLease<ListSlim<FacetResult>> facetResults;

        internal QueryResult(
            DocumentResultCollection<TDocument> documentResults,
            ObjectPoolLease<ListSlim<FacetResult>> facetResults)
        {
            this.documentResults = documentResults;
            this.facetResults = facetResults;
        }

        public IReadOnlyCollection<DocumentResult<TDocument>> Documents
        {
            get
            {
                return this.documentResults;
            }
        }

        public IReadOnlyCollection<FacetResult> Facets
        {
            get
            {
                return this.facetResults.Instance;
            }
        }

        public void Dispose()
        {
            foreach (var facetResult in this.facetResults.Instance)
            {
                (facetResult as IDisposable)?.Dispose();
            }

            this.documentResults.Dispose();
            this.facetResults.Dispose();
        }
    }
}
