using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishStopTokenFilter : StopTokenFilter
    {
        public LuceneEnglishStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = EnglishAnalyzer.DefaultStopSet.ToArray();
    }
}
