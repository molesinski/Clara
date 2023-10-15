namespace Clara.Analysis
{
    public sealed partial class Analyzer : IAnalyzer
    {
        private readonly IEnumerable<AnalyzerTerm> empty;
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

            this.empty = new AnalyzerTermEnumerable(this, string.Empty);
            this.tokenizer = tokenizer;
            this.pipeline = CreatePipeline(filters);
        }

        public ITokenizer Tokenizer
        {
            get
            {
                return this.tokenizer;
            }
        }

        public IEnumerable<AnalyzerTerm> GetTerms(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return this.empty;
            }

            return new AnalyzerTermEnumerable(this, text);
        }

        private static TokenFilterDelegate CreatePipeline(IEnumerable<ITokenFilter> filters)
        {
            TokenFilterDelegate pipeline =
                (ref Token token) =>
                {
                };

            foreach (var filter in filters.Reverse())
            {
                if (filter is not null)
                {
                    var next = pipeline;

                    pipeline =
                        (ref Token token) =>
                        {
                            filter.Process(ref token, next);
                        };
                }
            }

            return pipeline;
        }
    }
}
