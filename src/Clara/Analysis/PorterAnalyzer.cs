namespace Clara.Analysis
{
    public class PorterAnalyzer : IAnalyzer
    {
        private readonly Analyzer analyzer;

        public PorterAnalyzer()
            : this(keywords: Array.Empty<string>())
        {
        }

        public PorterAnalyzer(IEnumerable<string> keywords)
        {
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            this.analyzer =
                new Analyzer(
                    new BasicTokenizer(),
                    new LowerInvariantTokenFilter(),
                    new CachingTokenFilter(),
                    new PorterPossessiveTokenFilter(),
                    new PorterStopTokenFilter(),
                    new LengthKeywordTokenFilter(),
                    new DigitsKeywordTokenFilter(),
                    new KeywordTokenFilter(keywords),
                    new PorterStemTokenFilter());
        }

        public IEnumerable<string> GetTokens(string text)
        {
            return this.analyzer.GetTokens(text);
        }
    }
}
