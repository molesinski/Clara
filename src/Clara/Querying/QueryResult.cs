using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Collections;
using Clara.Storage;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly DocumentSort documentSort;
        private readonly PooledDictionary<int, TDocument> documents;
        private readonly List<FacetResult> facets;

        internal QueryResult(
            DocumentSort documentSort,
            PooledDictionary<int, TDocument> documents,
            List<FacetResult> facets)
        {
            if (documentSort is null)
            {
                throw new ArgumentNullException(nameof(documentSort));
            }

            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            if (facets is null)
            {
                throw new ArgumentNullException(nameof(facets));
            }

            this.documentSort = documentSort;
            this.documents = documents;
            this.facets = facets;
        }

        public IEnumerable<DocumentResult> Documents
        {
            get
            {
                return this.documentSort.Documents
                    .Select(o => new QueryResult<TDocument>.DocumentResult(this.documents.GetValueOrDefault(o), 1));
            }
        }

        public IEnumerable<FacetResult> Facets
        {
            get
            {
                return this.facets;
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
        }

        public readonly struct DocumentResult
        {
            public DocumentResult(TDocument document, double score)
            {
                this.Document = document;
                this.Score = score;
            }

            public TDocument Document { get; }

            public double Score { get; }
        }
    }
}
