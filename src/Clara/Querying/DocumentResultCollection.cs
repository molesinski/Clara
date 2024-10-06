using System.Collections;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class DocumentResultCollection<TDocument> : IReadOnlyList<DocumentResult<TDocument>>, IDisposable
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, TDocument> documentMap;
        private readonly DocumentScoring documentScoring;
        private readonly DocumentList documentList;
        private bool isDisposed;

        internal DocumentResultCollection(
            TokenEncoder tokenEncoder,
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
                this.ThrowIfDisposed();

                return this.documentList.Value.Count;
            }
        }

        public DocumentResult<TDocument> this[int index]
        {
            get
            {
                this.ThrowIfDisposed();

                var documentId = this.documentList.Value[index];
                var key = this.tokenEncoder.Decode(documentId);
                var document = this.documentMap[documentId];

                this.documentScoring.Value.TryGetValue(documentId, out var score);

                return new DocumentResult<TDocument>(key, document, score);
            }
        }

        public Enumerator GetEnumerator()
        {
            this.ThrowIfDisposed();

            return new Enumerator(this);
        }

        IEnumerator<DocumentResult<TDocument>> IEnumerable<DocumentResult<TDocument>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.documentScoring.Dispose();
                this.documentList.Dispose();

                this.isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        public struct Enumerator : IEnumerator<DocumentResult<TDocument>>
        {
            private readonly TokenEncoder tokenEncoder;
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
                this.documentScoring = documentResultCollection.documentScoring.Value;
                this.documentList = documentResultCollection.documentList.Value;
                this.count = documentResultCollection.documentList.Value.Count;
                this.index = 0;
                this.current = default;
            }

            public readonly DocumentResult<TDocument> Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
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
                    var document = this.documentMap[documentId];

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
                this.Reset();
            }
        }
    }
}
