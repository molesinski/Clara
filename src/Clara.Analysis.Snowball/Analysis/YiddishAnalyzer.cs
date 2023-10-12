using Snowball;

namespace Clara.Analysis
{
    public sealed class YiddishAnalyzer : SnowballAnalyzer<YiddishStemTokenFilter, YiddishStopTokenFilter, YiddishStemmer>
    {
        public YiddishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
