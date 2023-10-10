﻿namespace Clara.Analysis
{
    public sealed class PorterAnalyzer : IAnalyzer
    {
        private readonly IAnalyzer analyzer;

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
                    new PorterPossessiveTokenFilter(),
                    new PorterStopTokenFilter(),
                    new DigitsKeywordTokenFilter(),
                    new KeywordTokenFilter(keywords),
                    new PorterStemTokenFilter());
        }

        public ITokenizer Tokenizer
        {
            get
            {
                return this.analyzer.Tokenizer;
            }
        }

        public IEnumerable<AnalyzerTerm> GetTerms(string text)
        {
            return this.analyzer.GetTerms(text);
        }
    }
}
