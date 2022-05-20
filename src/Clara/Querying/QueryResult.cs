using System;
using System.Collections.Generic;
using Clara.Storage;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly BufferScope bufferScope;

        internal QueryResult(
            IEnumerable<DocumentResult> documents,
            IEnumerable<FacetResult> facets,
            int totalCount,
            BufferScope bufferScope)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            if (facets is null)
            {
                throw new ArgumentNullException(nameof(facets));
            }

            if (totalCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalCount));
            }

            if (bufferScope is null)
            {
                throw new ArgumentNullException(nameof(bufferScope));
            }

            this.bufferScope = bufferScope;

            this.Documents = documents;
            this.Facets = facets;
            this.TotalCount = totalCount;
        }

        public IEnumerable<DocumentResult> Documents { get; }

        public IEnumerable<FacetResult> Facets { get; }

        public int TotalCount { get; }

        public void Dispose()
        {
            this.bufferScope.Dispose();
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
