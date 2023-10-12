using Snowball;

namespace Clara.Analysis
{
    public sealed class RomanianAnalyzer : SnowballAnalyzer<RomanianStemTokenFilter, RomanianStopTokenFilter, RomanianStemmer>
    {
        public RomanianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
