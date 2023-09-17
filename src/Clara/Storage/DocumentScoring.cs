using Clara.Utils;

namespace Clara.Storage
{
    internal abstract class DocumentScoring : IDocumentScoring
    {
        public static DocumentScoring Empty { get; } = new EmptySearchResult();

        public abstract bool IsEmpty { get; }

        public static DocumentScoring From(ObjectPoolLease<DictionarySlim<int, float>> documentScores)
        {
            return new PooledDictionaryDocumentScoring(documentScores);
        }

        public static DocumentScoring From(DictionarySlim<int, float> documentScores)
        {
            return new DictionarySlimDocumentScoring(documentScores);
        }

        public abstract float GetScore(int documentId);

        public abstract void Dispose();

        private sealed class EmptySearchResult : DocumentScoring
        {
            public override bool IsEmpty
            {
                get
                {
                    return true;
                }
            }

            public override float GetScore(int documentId)
            {
                return 0;
            }

            public override void Dispose()
            {
            }
        }

        private sealed class PooledDictionaryDocumentScoring : DocumentScoring
        {
            private readonly ObjectPoolLease<DictionarySlim<int, float>> documentScores;

            public PooledDictionaryDocumentScoring(ObjectPoolLease<DictionarySlim<int, float>> documentScores)
            {
                this.documentScores = documentScores;
            }

            public override bool IsEmpty
            {
                get
                {
                    return false;
                }
            }

            public override float GetScore(int documentId)
            {
                if (this.documentScores.Instance.TryGetValue(documentId, out var score))
                {
                    return score;
                }

                return 0;
            }

            public override void Dispose()
            {
                this.documentScores.Dispose();
            }
        }

        private sealed class DictionarySlimDocumentScoring : DocumentScoring
        {
            private readonly DictionarySlim<int, float> documentScores;

            public DictionarySlimDocumentScoring(DictionarySlim<int, float> documentScores)
            {
                if (documentScores is null)
                {
                    throw new ArgumentNullException(nameof(documentScores));
                }

                this.documentScores = documentScores;
            }

            public override bool IsEmpty
            {
                get
                {
                    return false;
                }
            }

            public override float GetScore(int documentId)
            {
                if (this.documentScores.TryGetValue(documentId, out var score))
                {
                    return score;
                }

                return 0;
            }

            public override void Dispose()
            {
            }
        }
    }
}
