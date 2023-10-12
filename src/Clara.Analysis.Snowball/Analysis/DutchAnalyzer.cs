using Snowball;

namespace Clara.Analysis
{
    public sealed class DutchAnalyzer : SnowballAnalyzer<DutchStemTokenFilter, DutchStopTokenFilter, DutchStemmer>
    {
        public DutchAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
