using Clara.Utils;

namespace Clara.Mapping
{
    public sealed class BM25Weight : Weight
    {
        public BM25Weight(double k1 = 1.2, double b = 0.75)
        {
            if (k1 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k1));
            }

            if (b < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

            this.K1 = k1;
            this.B = b;
        }

        public double K1 { get; }

        public double B { get; }

        public override void Process(
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, int> documentLengths)
        {
            var k1 = this.K1;
            var b = this.B;

            var documentCount = documentLengths.Count;
            var averageLength = documentLengths.Average(x => x.Value);

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
