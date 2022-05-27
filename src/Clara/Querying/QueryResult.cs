using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly IDocumentSet documentSet;
        private readonly List<FieldFacetResult> facetResults;
        private readonly PooledDictionarySlim<int, TDocument> documents;

        internal QueryResult(
            IDocumentSet documentSet,
            List<FieldFacetResult> facetResults,
            PooledDictionarySlim<int, TDocument> documents)
        {
            if (documentSet is null)
            {
                throw new ArgumentNullException(nameof(documentSet));
            }

            if (facetResults is null)
            {
                throw new ArgumentNullException(nameof(facetResults));
            }

            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            this.documentSet = documentSet;
            this.facetResults = facetResults;
            this.documents = documents;
        }

        public int Count
        {
            get
            {
                return this.documentSet.Count;
            }
        }

        public IEnumerable<DocumentResult<TDocument>> Documents
        {
            get
            {
                return this.documentSet
                    .Select(
                        o =>
                        {
                            this.documents.TryGetValue(o, out var document);

                            return new DocumentResult<TDocument>(document, 1);
                        });
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

        public void Dispose()
        {
            this.documentSet.Dispose();

            foreach (var fieldFacet in this.facetResults)
            {
                fieldFacet.Dispose();
            }
        }
    }
}
