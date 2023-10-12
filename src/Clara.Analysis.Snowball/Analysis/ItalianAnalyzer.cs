using Snowball;

namespace Clara.Analysis
{
    public sealed class ItalianAnalyzer : SnowballAnalyzer<ItalianStemTokenFilter, ItalianStopTokenFilter, ItalianStemmer>
    {
        public ItalianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
