using Snowball;

namespace Clara.Analysis
{
    public sealed class ArabicAnalyzer : SnowballAnalyzer<ArabicStemTokenFilter, ArabicStopTokenFilter, ArabicStemmer>
    {
        public ArabicAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
