using Snowball;

namespace Clara.Analysis
{
    public sealed class NorwegianAnalyzer : SnowballAnalyzer<NorwegianStemTokenFilter, NorwegianStopTokenFilter, NorwegianStemmer>
    {
        public NorwegianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
