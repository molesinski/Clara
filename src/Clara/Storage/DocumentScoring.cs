using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentScoring : IDisposable
    {
        private static readonly ObjectPool<ListSlim<DocumentValue<float>>> ScoredDocumentListsPool = new(() => new());
        private static readonly DictionarySlim<int, float> Empty = new();

        private readonly ObjectPoolLease<DictionarySlim<int, float>>? lease;
        private readonly DictionarySlim<int, float>? instance;

        public DocumentScoring(ObjectPoolLease<DictionarySlim<int, float>> lease)
        {
            this.lease = lease;
            this.instance = null;
        }

        public DocumentScoring(DictionarySlim<int, float> instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.lease = null;
            this.instance = instance;
        }

        public DictionarySlim<int, float> Scoring
        {
            get
            {
                return this.lease?.Instance ?? this.instance ?? Empty;
            }
        }

        public DocumentList Sort(HashSetSlim<int> documentSet)
        {
            using var sortedDocuments = ScoredDocumentListsPool.Lease();

            var scoring = this.Scoring;

            foreach (var documentId in documentSet)
            {
                scoring.TryGetValue(documentId, out var value);

                sortedDocuments.Instance.Add(new DocumentValue<float>(documentId, value));
            }

            sortedDocuments.Instance.Sort(DocumentValueComparer<float>.Descending);

            var documents = SharedObjectPools.DocumentLists.Lease();

            foreach (var documentValue in sortedDocuments.Instance)
            {
                documents.Instance.Add(documentValue.DocumentId);
            }

            return new DocumentList(documents);
        }

        public void Dispose()
        {
            this.lease?.Dispose();
        }
    }
}
