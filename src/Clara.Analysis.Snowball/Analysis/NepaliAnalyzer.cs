using Snowball;

namespace Clara.Analysis
{
    public sealed class NepaliAnalyzer : SnowballAnalyzer<NepaliStemTokenFilter, NepaliStopTokenFilter, NepaliStemmer>
    {
        public NepaliAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
