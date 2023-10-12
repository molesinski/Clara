using Snowball;

namespace Clara.Analysis
{
    public sealed class SpanishAnalyzer : SnowballAnalyzer<SpanishStemTokenFilter, SpanishStopTokenFilter, SpanishStemmer>
    {
        public SpanishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
