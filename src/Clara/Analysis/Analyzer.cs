﻿namespace Clara.Analysis
{
    public sealed class Analyzer : IAnalyzer
    {
        private readonly ITokenizer tokenizer;
        private readonly TokenFilterDelegate pipeline;

        public Analyzer(ITokenizer tokenizer)
            : this(tokenizer, (IEnumerable<ITokenFilter>)Array.Empty<ITokenFilter>())
        {
        }

        public Analyzer(ITokenizer tokenizer, params ITokenFilter[] filters)
            : this(tokenizer, (IEnumerable<ITokenFilter>)filters)
        {
        }

        public Analyzer(ITokenizer tokenizer, IEnumerable<ITokenFilter> filters)
        {
            if (tokenizer is null)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            if (filters is null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            this.tokenizer = tokenizer;
            this.pipeline = CreatePipeline(filters);
        }

        public IEnumerable<string> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<string>();
            }

            return GetTokensEnumerable(text);

            IEnumerable<string> GetTokensEnumerable(string text)
            {
                foreach (var token in this.tokenizer.GetTokens(text))
                {
                    var result = this.pipeline(token);

                    if (result.Length > 0)
                    {
                        yield return result.ToString();
                    }
                }
            }
        }

        private static TokenFilterDelegate CreatePipeline(IEnumerable<ITokenFilter> filters)
        {
            TokenFilterDelegate pipeline =
                token =>
                {
                    return token;
                };

            foreach (var filter in filters.Reverse())
            {
                if (filter is not null)
                {
                    var next = pipeline;

                    pipeline =
                        token =>
                        {
                            return filter.Process(token, next);
                        };
                }
            }

            return pipeline;
        }
    }
}
