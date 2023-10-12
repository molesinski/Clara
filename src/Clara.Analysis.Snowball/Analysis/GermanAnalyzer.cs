using Snowball;

namespace Clara.Analysis
{
    public sealed class GermanAnalyzer : SnowballAnalyzer<GermanStemTokenFilter, GermanStopTokenFilter, GermanStemmer>
    {
        public GermanAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
