using System.Collections;
using Clara.Utils;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardTokenizer : ITokenizer
    {
        private static readonly IEnumerable<Token> Empty = new TokenEnumerable(string.Empty);

        internal static LuceneStandardTokenizer Instance { get; } = new();

        public IEnumerable<Token> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Empty;
            }

            return new TokenEnumerable(text);
        }

        public bool Equals(ITokenizer? other)
        {
            return other is LuceneStandardTokenizer;
        }

        internal readonly struct TokenEnumerable : IEnumerable<Token>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly string text;

            internal TokenEnumerable(string text)
            {
                this.text = text;
            }

            public IEnumerator<Token> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.text);

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

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Dispose returns object to pool for reuse")]
            private sealed class Enumerator : IEnumerator<Token>
            {
                private readonly ReusableStringReader reader;
                private readonly Tokenizer tokenizer;
                private readonly char[] chars;
                private ObjectPoolLease<Enumerator>? lease;
                private bool isEmpty;
                private Token current;
                private TokenStreamEnumerable.Enumerator enumerator;
                private bool isEnumeratorSet;

                public Enumerator()
                {
                    this.reader = new ReusableStringReader();
                    this.tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_48, this.reader);
                    this.chars = new char[Token.MaximumLength];
                }

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

                public void Initialize(ObjectPoolLease<Enumerator> lease, string text)
                {
                    this.lease = lease;
                    this.isEmpty = string.IsNullOrWhiteSpace(text);
                    this.current = default!;
                    this.enumerator = default;
                    this.isEnumeratorSet = false;

                    this.reader.SetText(text);
                    this.tokenizer.SetReader(this.reader);
                }

                public bool MoveNext()
                {
                    if (this.isEmpty)
                    {
                        this.current = default;

                        return false;
                    }

                    if (!this.isEnumeratorSet)
                    {
                        this.enumerator = new TokenStreamEnumerable(this.tokenizer, this.chars).GetEnumerator();
                        this.isEnumeratorSet = true;
                    }

                    while (this.enumerator.MoveNext())
                    {
                        var token = this.enumerator.Current;

                        if (!token.IsEmpty)
                        {
                            this.current = token;

                            return true;
                        }
                    }

                    this.current = default;

                    return false;
                }

                public void Reset()
                {
                    if (this.isEnumeratorSet)
                    {
                        this.enumerator.Dispose();
                        this.enumerator = default;
                    }

                    this.isEnumeratorSet = false;
                    this.current = default!;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.isEmpty = default;

                    var lease = this.lease;
                    this.lease = null;
                    lease?.Dispose();
                }
            }
        }
    }
}
