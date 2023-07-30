using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishStopTokenFilter : StopTokenFilter
    {
        public LuceneEnglishStopTokenFilter()
            : base(EnglishAnalyzer.DefaultStopSet)
        {
        }
    }
}
