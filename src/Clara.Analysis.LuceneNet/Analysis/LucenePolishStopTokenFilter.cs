using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStopTokenFilter : StopTokenFilter
    {
        public LucenePolishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = PolishAnalyzer.DefaultStopSet.ToArray();
    }
}
