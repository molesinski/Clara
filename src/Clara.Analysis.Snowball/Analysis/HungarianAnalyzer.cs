using Snowball;

namespace Clara.Analysis
{
    public sealed class HungarianAnalyzer : SnowballAnalyzer<HungarianStemTokenFilter, HungarianStopTokenFilter, HungarianStemmer>
    {
        public HungarianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
