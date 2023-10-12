using Snowball;

namespace Clara.Analysis
{
    public sealed class LithuanianAnalyzer : SnowballAnalyzer<LithuanianStemTokenFilter, LithuanianStopTokenFilter, LithuanianStemmer>
    {
        public LithuanianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
