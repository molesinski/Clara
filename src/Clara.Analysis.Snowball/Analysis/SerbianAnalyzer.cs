using Snowball;

namespace Clara.Analysis
{
    public sealed class SerbianAnalyzer : SnowballAnalyzer<SerbianStemTokenFilter, SerbianStopTokenFilter, SerbianStemmer>
    {
        public SerbianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
