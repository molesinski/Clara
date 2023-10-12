using Snowball;

namespace Clara.Analysis
{
    public sealed class HindiAnalyzer : SnowballAnalyzer<HindiStemTokenFilter, HindiStopTokenFilter, HindiStemmer>
    {
        public HindiAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
