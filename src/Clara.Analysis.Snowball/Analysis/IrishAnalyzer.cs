using Snowball;

namespace Clara.Analysis
{
    public sealed class IrishAnalyzer : SnowballAnalyzer<IrishStemTokenFilter, IrishStopTokenFilter, IrishStemmer>
    {
        public IrishAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
