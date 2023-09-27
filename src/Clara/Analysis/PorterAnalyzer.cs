namespace Clara.Analysis
{
    public class PorterAnalyzer : IAnalyzer
    {
        private readonly Analyzer analyzer;

        public PorterAnalyzer()
        {
            this.analyzer =
                new Analyzer(
                    new BasicTokenizer(),
                    new LowerInvariantTokenFilter(),
                    new CachingTokenFilter(),
                    new PorterPossessiveTokenFilter(),
                    new PorterStopTokenFilter(),
                    new KeywordLengthTokenFilter(),
                    new KeywordDigitsTokenFilter(),
                    new PorterStemTokenFilter());
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
                    new KeywordLengthTokenFilter(),
                    new KeywordDigitsTokenFilter(),
                    new KeywordTokenFilter(keywords),
                    new PorterStemTokenFilter());
        }

        public IEnumerable<string> GetTokens(string text)
        {
            return this.analyzer.GetTokens(text);
        }
    }
}
