using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class Similarity
    {
        public static Similarity Default { get; } = BM25();

        public static Similarity TF { get; } = new TFSimilarity();

        public static Similarity TFIDF { get; } = new TFIDFSimilarity();

        public static Similarity BM25(float k1 = 1.2f, float b = 0.75f)
        {
            return new BM25Similarity(k1, b);
        }

        public abstract void Transform(
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, float> documentLengths);

        private sealed class TFSimilarity : Similarity
        {
            public override void Transform(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, float> documentLengths)
            {
            }
        }

        private sealed class TFIDFSimilarity : Similarity
        {
            public override void Transform(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, float> documentLengths)
            {
                var count = documentLengths.Count;

                if (count == 0)
                {
                    return;
                }

                foreach (var tokenDocumentScoresItem in tokenDocumentScores)
                {
                    var documentScores = tokenDocumentScoresItem.Value;
                    var df = documentScores.Count;
                    var idf = MathF.Log(1f + ((float)count / df));

                    foreach (var documentScoresItem in documentScores)
                    {
                        var documentId = documentScoresItem.Key;

                        ref var score = ref documentScores.GetValueRefOrAddDefault(documentId, out _);

                        score = score * idf;
                    }
                }
            }
        }

        private sealed class BM25Similarity : Similarity
        {
            private readonly float k1;
            private readonly float b;

            public BM25Similarity(float k1 = 1.2f, float b = 0.75f)
            {
                if (k1 < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(k1));
                }

                if (b < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(b));
                }

                this.k1 = k1;
                this.b = b;
            }

            public override void Transform(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, float> documentLengths)
            {
                var count = documentLengths.Count;

                if (count == 0)
                {
                    return;
                }

                var k1 = this.k1;
                var b = this.b;
                var avgLen = 0.0f;

                foreach (var pair in documentLengths)
                {
                    avgLen += pair.Value;
                }

                avgLen /= count;

                foreach (var tokenDocumentScoresItem in tokenDocumentScores)
                {
                    var documentScores = tokenDocumentScoresItem.Value;
                    var df = documentScores.Count;
                    var idf = MathF.Log(1f + ((count - df + 0.5f) / (df + 0.5f)));

                    foreach (var documentScoresItem in documentScores)
                    {
                        var documentId = documentScoresItem.Key;
                        var len = documentLengths[documentId];

                        ref var score = ref documentScores.GetValueRefOrAddDefault(documentId, out _);

                        score = score * (k1 + 1f) / (score + (k1 * (1f - b + (b * (len / avgLen))))) * idf;
                    }
                }
            }
        }
    }
}
