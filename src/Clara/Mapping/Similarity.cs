using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class Similarity
    {
        internal Similarity()
        {
        }

        public static Similarity None { get; } = new NoneTextSimilarity();

        public static Similarity Default { get; } = BM25();

        public static Similarity BM25(double k1 = 1.2, double b = 0.75)
        {
            return new BM25TextSimilarity(k1, b);
        }

        internal abstract void Process(
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, float> documentLengths);

        private sealed class NoneTextSimilarity : Similarity
        {
            internal override void Process(DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores, DictionarySlim<int, float> documentLengths)
            {
            }
        }

        private sealed class BM25TextSimilarity : Similarity
        {
            private readonly double k1;
            private readonly double b;

            public BM25TextSimilarity(double k1 = 1.2, double b = 0.75)
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

            internal override void Process(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, float> documentLengths)
            {
                var documentCount = documentLengths.Count;

                if (documentCount == 0)
                {
                    return;
                }

                var k1 = this.k1;
                var b = this.b;
                var averageLength = 0.0;

                foreach (var pair in documentLengths)
                {
                    averageLength += pair.Value;
                }

                averageLength /= documentCount;

                foreach (var tokenDocumentScoresItem in tokenDocumentScores)
                {
                    var documentScores = tokenDocumentScoresItem.Value;
                    var documentFrequency = documentScores.Count;
                    var inverseDocumentFrequency = Math.Log(1 + ((documentCount - documentFrequency + 0.5) / (documentFrequency + 0.5)));

                    foreach (var documentScoresItem in documentScores)
                    {
                        var documentId = documentScoresItem.Key;

                        ref var score = ref documentScores.GetValueRefOrAddDefault(documentId, out _);

                        var frequency = score;
                        var length = documentLengths[documentId];
                        var weighted = inverseDocumentFrequency * ((frequency * (k1 + 1.0)) / (frequency + (k1 * (1.0 - b + (b * (length / averageLength))))));

                        score = (float)weighted;
                    }
                }
            }
        }
    }
}
