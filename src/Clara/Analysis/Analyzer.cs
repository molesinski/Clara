using System;
using System.Collections.Generic;
using System.Linq;

namespace Clara.Analysis
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

        public static Analyzer Empty { get; } = new Analyzer(new EmptyTokenizer());

        public IEnumerable<string> GetTokens(string text)
        {
            foreach (var token in this.tokenizer.GetTokens(text))
            {
                var result = this.pipeline(token);

                if (!result.IsEmpty)
                {
                    yield return result.ToString();
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
                var next = pipeline;

                pipeline =
                    token =>
                    {
                        return filter.Process(token, next);
                    };
            }

            return pipeline;
        }

        private class EmptyTokenizer : ITokenizer
        {
            public IEnumerable<Token> GetTokens(string text)
            {
                return Array.Empty<Token>();
            }
        }
    }
}
