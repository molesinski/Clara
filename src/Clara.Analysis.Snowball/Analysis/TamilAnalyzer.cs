using Snowball;

namespace Clara.Analysis
{
    public sealed class TamilAnalyzer : SnowballAnalyzer<TamilStemTokenFilter, TamilStopTokenFilter, TamilStemmer>
    {
        public TamilAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
