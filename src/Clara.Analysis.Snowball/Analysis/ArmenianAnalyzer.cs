using Snowball;

namespace Clara.Analysis
{
    public sealed class ArmenianAnalyzer : SnowballAnalyzer<ArmenianStemTokenFilter, ArmenianStopTokenFilter, ArmenianStemmer>
    {
        public ArmenianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
