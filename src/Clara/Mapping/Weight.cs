using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class Weight
    {
        protected internal Weight()
        {
        }

        public static Weight BM25 { get; } = new BM25Weight();

        internal abstract void Process(
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, int> documentLengths);

        private sealed class BM25Weight : Weight
        {
            internal override void Process(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, int> documentLengths)
            {
                var k1 = 1.2;
                var b = 0.75;
                var docCount = documentLengths.Count;
                var avgFieldLen = documentLengths.Average(x => x.Value);

                foreach (var tokenDocumentScoresItem in tokenDocumentScores)
                {
                    var documentScores = tokenDocumentScoresItem.Value;

                    var fqi = documentScores.Count;
                    var idf = Math.Log(1 + ((docCount - fqi + 0.5) / (fqi + 0.5)));

                    foreach (var documentScoresItem in documentScores)
                    {
                        var documentId = documentScoresItem.Key;

                        ref var score = ref documentScores.GetValueRefOrAddDefault(documentId, out _);

                        var fqid = score;
                        var fieldLen = documentLengths[documentId];
                        var weighted = idf * ((fqid * (k1 + 1.0)) / (fqid + (k1 * (1.0 - b + (b * (fieldLen / avgFieldLen))))));

                        score = (float)weighted;
                    }
                }
            }
        }
    }
}
