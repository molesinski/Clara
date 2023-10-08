using Clara.Utils;

namespace Clara.Mapping
{
    public abstract partial class Similarity
    {
        private sealed class TFIDFSimilarity : Similarity
        {
            internal override void Process(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, float> documentLengths)
            {
                var documentCount = documentLengths.Count;

                if (documentCount == 0)
                {
                    return;
                }

                foreach (var tokenDocumentScoresItem in tokenDocumentScores)
                {
                    var documentScores = tokenDocumentScoresItem.Value;
                    var documentFrequency = documentScores.Count;
                    var inverseDocumentFrequency = Math.Log(1 + (documentCount / documentFrequency));

                    foreach (var documentScoresItem in documentScores)
                    {
                        var documentId = documentScoresItem.Key;

                        ref var score = ref documentScores.GetValueRefOrAddDefault(documentId, out _);

                        var weighted = score * inverseDocumentFrequency;

                        score = (float)weighted;
                    }
                }
            }
        }
    }
}
