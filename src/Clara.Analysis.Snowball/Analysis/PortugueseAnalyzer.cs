using Snowball;

namespace Clara.Analysis
{
    public sealed class PortugueseAnalyzer : SnowballAnalyzer<PortugueseStemTokenFilter, PortugueseStopTokenFilter, PortugueseStemmer>
    {
        public PortugueseAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
