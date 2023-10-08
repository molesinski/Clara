using Clara.Utils;

namespace Clara.Mapping
{
    public abstract partial class Similarity
    {
        internal Similarity()
        {
        }

        public static Similarity None { get; } = new NoneSimilarity();

        public static Similarity Default { get; } = BM25();

        public static Similarity TFIDF { get; } = new TFIDFSimilarity();

        public static Similarity BM25(double k1 = 1.2, double b = 0.75)
        {
            return new BM25Similarity(k1, b);
        }

        internal abstract void Process(
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, float> documentLengths);

        private sealed class NoneSimilarity : Similarity
        {
            internal override void Process(
                DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
                DictionarySlim<int, float> documentLengths)
            {
            }
        }
    }
}
