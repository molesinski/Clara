using Snowball;

namespace Clara.Analysis
{
    public sealed class CatalanAnalyzer : SnowballAnalyzer<CatalanStemTokenFilter, CatalanStopTokenFilter, CatalanStemmer>
    {
        public CatalanAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
