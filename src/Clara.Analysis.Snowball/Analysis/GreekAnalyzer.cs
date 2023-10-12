using Snowball;

namespace Clara.Analysis
{
    public sealed class GreekAnalyzer : SnowballAnalyzer<GreekStemTokenFilter, GreekStopTokenFilter, GreekStemmer>
    {
        public GreekAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
