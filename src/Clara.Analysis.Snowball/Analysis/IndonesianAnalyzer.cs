using Snowball;

namespace Clara.Analysis
{
    public sealed class IndonesianAnalyzer : SnowballAnalyzer<IndonesianStemTokenFilter, IndonesianStopTokenFilter, IndonesianStemmer>
    {
        public IndonesianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
