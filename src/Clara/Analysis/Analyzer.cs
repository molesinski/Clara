namespace Clara.Analysis
{
    public sealed partial class Analyzer : IAnalyzer
    {
        private readonly IEnumerable<string> emptyEnumerable;
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

            this.emptyEnumerable = new TokenEnumerable(this, string.Empty);
            this.tokenizer = tokenizer;
            this.pipeline = CreatePipeline(filters);
        }

        public TokenEnumerable GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new TokenEnumerable(this, text);
        }

        IEnumerable<string> IAnalyzer.GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return this.emptyEnumerable;
            }

            return new TokenEnumerable(this, text);
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
