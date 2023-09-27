using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishStopTokenFilter : StopTokenFilter
    {
        public LuceneEnglishStopTokenFilter()
            : base(Stopwords)
        {
        }

        public static IReadOnlyCollection<string> Stopwords { get; } = EnglishAnalyzer.DefaultStopSet.ToArray();
    }
}
