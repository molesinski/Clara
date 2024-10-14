﻿using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class EstonianAnalyzer : IAnalyzer
    {
        private readonly Analyzer analyzer;

        public EstonianAnalyzer(
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
                filters.Add(new EstonianStopTokenFilter());
            }

            if (keywords is not null)
            {
                filters.Add(new KeywordTokenFilter(keywords));
            }

            filters.Add(new EstonianStemTokenFilter());

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