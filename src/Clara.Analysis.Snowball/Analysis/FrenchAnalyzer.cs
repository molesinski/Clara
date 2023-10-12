using Snowball;

namespace Clara.Analysis
{
    public sealed class FrenchAnalyzer : SnowballAnalyzer<FrenchStemTokenFilter, FrenchStopTokenFilter, FrenchStemmer>
    {
        public FrenchAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
                : base(stopwords: stopwords, keywords: keywords)
        {
        }
    }
}
