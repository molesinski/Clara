using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class Weight
    {
        internal Weight()
        {
        }

        public static Weight Default { get; } = BM25();

        public static Weight BM25(double k1 = 1.2, double b = 0.75)
        {
            return new BM25Weight(k1, b);
        }

        internal abstract void Process(
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, int> documentLengths);
    }
}
