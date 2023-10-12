using Snowball;

namespace Clara.Analysis
{
    public sealed class RussianAnalyzer : SnowballAnalyzer<RussianStemTokenFilter, RussianStopTokenFilter, RussianStemmer>
    {
        public RussianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
