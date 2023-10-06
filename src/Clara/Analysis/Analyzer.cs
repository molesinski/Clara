using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class Analyzer : IAnalyzer
    {
        private readonly ObjectPool<TokenEnumerable> enumerablePool;
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

            this.enumerablePool = new(() => new(this));
            this.tokenizer = tokenizer;
            this.pipeline = CreatePipeline(filters);
        }

        public IDisposableEnumerable<string> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return DisposableEnumerable<string>.Empty;
            }

            var enumerable = this.enumerablePool.Lease();

            enumerable.Instance.Initialize(text, enumerable);

            return enumerable.Instance;
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

        private sealed class TokenEnumerable : IDisposableEnumerable<string>
        {
            private readonly Enumerator enumerator;
            private ObjectPoolLease<TokenEnumerable> lease;
            private bool isDisposed;

            public TokenEnumerable(Analyzer analyzer)
            {
                this.enumerator = new Enumerator(analyzer);
                this.lease = default;
                this.isDisposed = true;
            }

            public void Initialize(string text, ObjectPoolLease<TokenEnumerable> lease)
            {
                if (!this.isDisposed)
                {
                    throw new InvalidOperationException("Current object instance is already initialized.");
                }

                this.enumerator.Initialize(text);
                this.lease = lease;
                this.isDisposed = false;
            }

            public Enumerator GetEnumerator()
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.enumerator;
            }

            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            void IDisposable.Dispose()
            {
                if (!this.isDisposed)
                {
                    this.enumerator.Dispose();
                    this.lease.Dispose();
                    this.lease = default;
                    this.isDisposed = true;
                }
            }

            public sealed class Enumerator : IEnumerator<string>
            {
                private readonly ITokenizer tokenizer;
                private readonly TokenFilterDelegate pipeline;
                private string text;
                private IDisposableEnumerable<Token>? tokensEnumerable;
                private IEnumerator<Token>? tokensEnumerator;
                private string current;

                public Enumerator(Analyzer analyzer)
                {
                    this.tokenizer = analyzer.tokenizer;
                    this.pipeline = analyzer.pipeline;
                    this.text = default!;
                    this.tokensEnumerable = null;
                    this.tokensEnumerator = null;
                    this.current = default!;
                }

                public string Current
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

                public void Initialize(string text)
                {
                    this.text = text;
                }

                public bool MoveNext()
                {
                    if (this.tokensEnumerator is null)
                    {
                        this.tokensEnumerable = this.tokenizer.GetTokens(this.text);
                        this.tokensEnumerator = this.tokensEnumerable.GetEnumerator();
                    }

                    while (this.tokensEnumerator.MoveNext())
                    {
                        var token = this.pipeline(this.tokensEnumerator.Current);

                        if (token.Length > 0)
                        {
                            this.current = token.ToString();

                            return true;
                        }
                    }

                    this.current = default!;

                    return false;
                }

                public void Reset()
                {
                    this.tokensEnumerator?.Dispose();
                    this.tokensEnumerator = null;
                    this.tokensEnumerable?.Dispose();
                    this.tokensEnumerable = null;
                    this.current = default!;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.text = default!;
                }
            }
        }
    }
}
