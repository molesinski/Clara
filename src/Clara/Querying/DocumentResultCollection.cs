using System.Collections;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    internal sealed class DocumentResultCollection<TDocument> : IReadOnlyCollection<DocumentResult<TDocument>>, IDisposable
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, TDocument> documentMap;
        private readonly DocumentScoring documentScoring;
        private readonly DocumentList documentList;

        public DocumentResultCollection(
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, TDocument> documentMap,
            DocumentScoring documentScoring,
            DocumentList documentList)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documentMap is null)
            {
                throw new ArgumentNullException(nameof(documentMap));
            }

            this.tokenEncoder = tokenEncoder;
            this.documentMap = documentMap;
            this.documentScoring = documentScoring;
            this.documentList = documentList;
        }

        public int Count
        {
            get
            {
                return this.documentList.List.Count;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<DocumentResult<TDocument>> IEnumerable<DocumentResult<TDocument>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void Dispose()
        {
            this.documentScoring.Dispose();
            this.documentList.Dispose();
        }

        public struct Enumerator : IEnumerator<DocumentResult<TDocument>>
        {
            private readonly ITokenEncoder tokenEncoder;
            private readonly DictionarySlim<int, TDocument> documentMap;
            private readonly DictionarySlim<int, float> documentScoring;
            private readonly ListSlim<int> documentList;
            private readonly int count;
            private int index;
            private DocumentResult<TDocument> current;

            internal Enumerator(DocumentResultCollection<TDocument> documentResultCollection)
            {
                this.tokenEncoder = documentResultCollection.tokenEncoder;
                this.documentMap = documentResultCollection.documentMap;
                this.documentScoring = documentResultCollection.documentScoring.Scoring;
                this.documentList = documentResultCollection.documentList.List;
                this.count = documentResultCollection.documentList.List.Count;
                this.index = 0;
                this.current = default;
            }

            public DocumentResult<TDocument> Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public bool MoveNext()
            {
                if (this.index < this.count)
                {
                    var documentId = this.documentList[this.index];
                    var key = this.tokenEncoder.Decode(documentId);
                    this.documentMap.TryGetValue(documentId, out var document);
                    this.documentScoring.TryGetValue(documentId, out var score);

                    this.current = new DocumentResult<TDocument>(key, document, score);
                    this.index++;

                    return true;
                }

                this.index = this.count + 1;
                this.current = default!;

                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default;
            }

            public void Dispose()
            {
            }
        }
    }
}
