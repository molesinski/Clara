using Snowball;

namespace Clara.Analysis
{
    public sealed class TurkishAnalyzer : SnowballAnalyzer<TurkishStemTokenFilter, TurkishStopTokenFilter, TurkishStemmer>
    {
        public TurkishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
