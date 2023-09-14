using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class DocumentScoring : IDocumentScoring
    {
        private readonly PooledDictionary<int, float> documentScores;

        public DocumentScoring(PooledDictionary<int, float> documentScores)
        {
            if (documentScores is null)
            {
                throw new ArgumentNullException(nameof(documentScores));
            }

            this.documentScores = documentScores;
        }

        public static IDocumentScoring Empty { get; } = new EmptySearchResult();

        public bool IsEmpty
        {
            get
            {
                return false;
            }
        }

        public float GetScore(int documentId)
        {
            if (this.documentScores.TryGetValue(documentId, out var score))
            {
                return score;
            }

            return 0;
        }

        public void Dispose()
        {
            this.documentScores.Dispose();
        }

        private sealed class EmptySearchResult : IDocumentScoring
        {
            public bool IsEmpty
            {
                get
                {
                    return true;
                }
            }

            public float GetScore(int documentId)
            {
                return 0;
            }

            public void Dispose()
            {
            }
        }
    }
}
