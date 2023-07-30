using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStopTokenFilter : StopTokenFilter
    {
        public LucenePolishStopTokenFilter()
            : base(PolishAnalyzer.DefaultStopSet)
        {
        }
    }
}
