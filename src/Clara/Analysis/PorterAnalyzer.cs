﻿using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class PorterAnalyzer : IAnalyzer
    {
        private readonly IAnalyzer analyzer;

        public PorterAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
        {
            var filters = new ListSlim<ITokenFilter>();

            filters.Add(new LowerInvariantTokenFilter());
            filters.Add(new EnglishPossessiveTokenFilter());

            if (stopwords is not null)
            {
                filters.Add(new StopTokenFilter(stopwords));
            }
            else
            {
                filters.Add(new PorterStopTokenFilter());
            }

            if (keywords is not null)
            {
                filters.Add(new KeywordTokenFilter(keywords));
            }

            filters.Add(new PorterStemTokenFilter());

            this.analyzer = new Analyzer(new BasicTokenizer(), filters);
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
