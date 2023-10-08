using Clara.Utils;

namespace Clara.Mapping
{
    public abstract partial class Similarity
    {
        private sealed class BM25Similarity : Similarity
        {
            private readonly double k1;
            private readonly double b;

            public BM25Similarity(double k1 = 1.2, double b = 0.75)
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
                var averageDocumentLength = 0.0;

                foreach (var pair in documentLengths)
                {
                    averageDocumentLength += pair.Value;
                }

                averageDocumentLength /= documentCount;

                foreach (var tokenDocumentScoresItem in tokenDocumentScores)
                {
                    var documentScores = tokenDocumentScoresItem.Value;
                    var documentFrequency = documentScores.Count;
                    var inverseDocumentFrequency = Math.Log(1 + ((documentCount - documentFrequency + 0.5) / (documentFrequency + 0.5)));

                    foreach (var documentScoresItem in documentScores)
                    {
                        var documentId = documentScoresItem.Key;
                        var documentLength = documentLengths[documentId];

                        ref var score = ref documentScores.GetValueRefOrAddDefault(documentId, out _);

                        var weighted = ((score * (k1 + 1.0)) / (score + (k1 * (1.0 - b + (b * (documentLength / averageDocumentLength)))))) * inverseDocumentFrequency;

                        score = (float)weighted;
                    }
                }
            }
        }
    }
}
