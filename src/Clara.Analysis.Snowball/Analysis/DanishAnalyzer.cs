using Snowball;

namespace Clara.Analysis
{
    public sealed class DanishAnalyzer : SnowballAnalyzer<DanishStemTokenFilter, DanishStopTokenFilter, DanishStemmer>
    {
        public DanishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
