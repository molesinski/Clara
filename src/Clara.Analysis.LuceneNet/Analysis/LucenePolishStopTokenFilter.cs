using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStopTokenFilter : StopTokenFilter
    {
        public LucenePolishStopTokenFilter()
            : base(Stopwords)
        {
        }

        public static IReadOnlyCollection<string> Stopwords { get; } = PolishAnalyzer.DefaultStopSet.ToArray();
    }
}
