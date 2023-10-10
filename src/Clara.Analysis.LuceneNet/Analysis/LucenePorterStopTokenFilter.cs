using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LucenePorterStopTokenFilter : StopTokenFilter
    {
        public LucenePorterStopTokenFilter()
            : base(DefaultStopwords)
        {
        }

        public static IReadOnlyCollection<string> DefaultStopwords { get; } = EnglishAnalyzer.DefaultStopSet.ToArray();
    }
}
