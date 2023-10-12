using Snowball;

namespace Clara.Analysis
{
    public sealed class BasqueAnalyzer : SnowballAnalyzer<BasqueStemTokenFilter, BasqueStopTokenFilter, BasqueStemmer>
    {
        public BasqueAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
