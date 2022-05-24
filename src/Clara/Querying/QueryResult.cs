using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Collections;
using Clara.Storage;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly PooledDictionary<int, TDocument> documents;
        private readonly DocumentSort documentSort;
        private readonly List<FieldFacetResult> facetResults;

        internal QueryResult(
            PooledDictionary<int, TDocument> documents,
            DocumentSort documentSort,
            List<FieldFacetResult> facetResults)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            if (documentSort is null)
            {
                throw new ArgumentNullException(nameof(documentSort));
            }

            if (facetResults is null)
            {
                throw new ArgumentNullException(nameof(facetResults));
            }

            this.documents = documents;
            this.documentSort = documentSort;
            this.facetResults = facetResults;
        }

        public IEnumerable<DocumentResult<TDocument>> Documents
        {
            get
            {
                return this.documentSort.Documents
                    .Select(o => new DocumentResult<TDocument>(this.documents.GetValueOrDefault(o), 1));
            }
        }

        public IEnumerable<FacetResult> Facets
        {
            get
            {
                return this.facetResults
                    .Select(o => o.FacetResult);
            }
        }

        public int TotalCount
        {
            get
            {
                return this.documentSort.Count;
            }
        }

        public void Dispose()
        {
            this.documentSort.Dispose();

            foreach (var fieldFacet in this.facetResults)
            {
                fieldFacet.Dispose();
            }
        }
    }
}
