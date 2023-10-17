using System.Collections;

namespace Clara.Analysis
{
    public sealed partial class Analyzer : IAnalyzer
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

        public ITokenizer Tokenizer
        {
            get
            {
                return this.tokenizer;
            }
        }

        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource(this);
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

        private sealed class TokenTermSource : ITokenTermSource, IEnumerable<TokenTerm>, IEnumerator<TokenTerm>
        {
            private readonly ITokenTermSource tokenTermSource;
            private readonly TokenFilterDelegate pipeline;
            private string text = string.Empty;
            private TokenTerm current;
            private IEnumerator<TokenTerm>? enumerator;

            internal TokenTermSource(Analyzer analyzer)
            {
                this.tokenTermSource = analyzer.Tokenizer.CreateTokenTermSource();
                this.pipeline = analyzer.pipeline;
            }

            TokenTerm IEnumerator<TokenTerm>.Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public IEnumerable<TokenTerm> GetTerms(string text)
            {
                if (text is null)
                {
                    throw new ArgumentNullException(nameof(text));
                }

                this.text = text;

                ((IEnumerator)this).Reset();

                return this;
            }

            IEnumerator<TokenTerm> IEnumerable<TokenTerm>.GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            bool IEnumerator.MoveNext()
            {
                this.enumerator ??= this.tokenTermSource.GetTerms(this.text).GetEnumerator();

                while (this.enumerator.MoveNext())
                {
                    var current = this.enumerator.Current;
                    var token = current.Token;

                    token = this.pipeline(token);

                    if (!token.IsEmpty)
                    {
                        this.current = new TokenTerm(token, current.Offset);
                        return true;
                    }
                }

                this.current = default;
                return false;
            }

            void IEnumerator.Reset()
            {
                this.current = default;
                this.enumerator?.Dispose();
                this.enumerator = default;
            }

            void IDisposable.Dispose()
            {
                ((IEnumerator)this).Reset();

                this.text = string.Empty;
            }
        }
    }
}
