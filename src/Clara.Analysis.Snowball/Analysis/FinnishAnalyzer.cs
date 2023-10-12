using Snowball;

namespace Clara.Analysis
{
    public sealed class FinnishAnalyzer : SnowballAnalyzer<FinnishStemTokenFilter, FinnishStopTokenFilter, FinnishStemmer>
    {
        public FinnishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
