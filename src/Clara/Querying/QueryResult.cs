using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class QueryResult<TDocument> : IDisposable
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, TDocument> documents;
        private readonly IDocumentSet documentSet;
        private readonly IDocumentScoring documentScoring;
        private readonly IEnumerable<FacetResult> facetResults;

        internal QueryResult(
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, TDocument> documents,
            IDocumentSet documentSet,
            IDocumentScoring documentScoring,
            IEnumerable<FacetResult> facetResults)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            if (documentSet is null)
            {
                throw new ArgumentNullException(nameof(documentSet));
            }

            if (documentScoring is null)
            {
                throw new ArgumentNullException(nameof(documentScoring));
            }

            if (facetResults is null)
            {
                throw new ArgumentNullException(nameof(facetResults));
            }

            this.tokenEncoder = tokenEncoder;
            this.documentSet = documentSet;
            this.documentScoring = documentScoring;
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
                foreach (var documentId in this.documentSet)
                {
                    var key = this.tokenEncoder.Decode(documentId);
                    var document = this.documents[documentId];
                    var score = this.documentScoring.GetScore(documentId);

                    yield return new DocumentResult<TDocument>(key, document, score);
                }
            }
        }

        public IEnumerable<FacetResult> Facets
        {
            get
            {
                return this.facetResults;
            }
        }

        public void Dispose()
        {
            this.documentSet.Dispose();
            this.documentScoring.Dispose();

            foreach (var facetResult in this.facetResults)
            {
                if (facetResult is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
