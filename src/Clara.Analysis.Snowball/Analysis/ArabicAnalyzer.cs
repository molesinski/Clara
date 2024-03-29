﻿using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class ArabicAnalyzer : IAnalyzer
    {
        private readonly Analyzer analyzer;

        public ArabicAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
        {
            var filters = new ListSlim<ITokenFilter>();

            filters.Add(new LowerInvariantTokenFilter());

            if (stopwords is not null)
            {
                filters.Add(new StopTokenFilter(stopwords));
            }
            else
            {
                filters.Add(new ArabicStopTokenFilter());
            }

            if (keywords is not null)
            {
                filters.Add(new KeywordTokenFilter(keywords));
            }

            filters.Add(new ArabicStemTokenFilter());

            this.analyzer = new Analyzer(new StandardTokenizer(), filters);
        }

        public ITokenizer Tokenizer
        {
            get
            {
                return this.analyzer.Tokenizer;
            }
        }

        public ITokenTermSource CreateTokenTermSource()
        {
            return this.analyzer.CreateTokenTermSource();
        }
    }
}
