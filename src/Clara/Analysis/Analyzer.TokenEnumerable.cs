using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed partial class Analyzer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design")]
        public readonly record struct TokenEnumerable : IEnumerable<Token>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly Analyzer analyzer;
            private readonly string text;

            internal TokenEnumerable(Analyzer analyzer, string text)
            {
                this.analyzer = analyzer;
                this.text = text;
            }

            public IEnumerator<Token> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.analyzer, this.text);

                return lease.Instance;
            }

            IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private sealed class Enumerator : IEnumerator<Token>
            {
                private ObjectPoolLease<Enumerator>? lease;
                private ITokenizer tokenizer = default!;
                private TokenFilterDelegate pipeline = default!;
                private string text = default!;
                private bool isEmpty;
                private Token current;
                private IEnumerator<Token>? enumerator;

                public Token Current
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

                public void Initialize(ObjectPoolLease<Enumerator> lease, Analyzer analyzer, string text)
                {
                    this.lease = lease;
                    this.tokenizer = analyzer.tokenizer;
                    this.pipeline = analyzer.pipeline;
                    this.text = text;
                    this.isEmpty = string.IsNullOrWhiteSpace(text);
                    this.current = default!;
                    this.enumerator = null;
                }

                public bool MoveNext()
                {
                    if (this.isEmpty)
                    {
                        this.current = default!;

                        return false;
                    }

                    this.enumerator ??= this.tokenizer.GetTokens(this.text).GetEnumerator();

                    while (this.enumerator.MoveNext())
                    {
                        var token = this.enumerator.Current;

                        token = this.pipeline(token);

                        if (token.Length > 0)
                        {
                            this.current = token;

                            return true;
                        }
                    }

                    this.current = default!;

                    return false;
                }

                public void Reset()
                {
                    this.enumerator?.Dispose();
                    this.enumerator = null;
                    this.current = default!;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.lease?.Dispose();
                    this.lease = null;
                    this.tokenizer = default!;
                    this.pipeline = default!;
                    this.text = default!;
                    this.isEmpty = default;
                }
            }
        }
    }
}
