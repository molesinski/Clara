namespace Clara.Analysis
{
    public class PorterAnalyzer : IAnalyzer
    {
        private readonly IAnalyzer analyzer =
            new Analyzer(
                new BasicTokenizer(),
                new LowerInvariantTokenFilter(),
                new CachingTokenFilter(),
                new PorterPossessiveTokenFilter(),
                new PorterStopTokenFilter(),
                new KeywordLengthTokenFilter(),
                new KeywordDigitsTokenFilter(),
                new PorterStemTokenFilter());

        public IEnumerable<string> GetTokens(string text)
        {
            return this.analyzer.GetTokens(text);
        }
    }
}
