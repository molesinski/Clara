using Snowball;

namespace Clara.Analysis
{
    public sealed class SwedishAnalyzer : SnowballAnalyzer<SwedishStemTokenFilter, SwedishStopTokenFilter, SwedishStemmer>
    {
        public SwedishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
