using Snowball;

namespace Clara.Analysis
{
    public sealed class EnglishAnalyzer : SnowballAnalyzer<EnglishStemTokenFilter, EnglishStopTokenFilter, EnglishStemmer>
    {
        public EnglishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
