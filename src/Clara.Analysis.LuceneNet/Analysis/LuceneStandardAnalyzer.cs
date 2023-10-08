using System.Collections;
using Clara.Utils;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public class LuceneStandardAnalyzer : IAnalyzer
    {
        private readonly IEnumerable<string> emptyEnumerable;

        public LuceneStandardAnalyzer()
        {
            this.emptyEnumerable = new TokenEnumerable(string.Empty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By design")]
        public TokenEnumerable GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new TokenEnumerable(text);
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

            return new TokenEnumerable(text);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design")]
        public readonly record struct TokenEnumerable : IEnumerable<string>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly string text;

            internal TokenEnumerable(string text)
            {
                this.text = text;
            }

            public IEnumerator<string> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.text);

                return lease.Instance;
            }

            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private sealed class Enumerator : IEnumerator<string>
            {
#pragma warning disable CA2213 // Disposable fields should be disposed
                private readonly ReusableStringReader reader;
                private readonly Lucene.Net.Analysis.Analyzer analyzer;
#pragma warning restore CA2213 // Disposable fields should be disposed
                private readonly char[] buffer;
                private ObjectPoolLease<Enumerator>? lease;
                private bool isEmpty;
                private string current = default!;
                private TokenStreamEnumerable.Enumerator enumerator;
                private bool isEnumeratorSet;

                public Enumerator()
                {
                    this.reader = new ReusableStringReader();
                    this.analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                    this.buffer = new char[Token.MaximumLength];
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

                public void Initialize(ObjectPoolLease<Enumerator> lease, string text)
                {
                    this.lease = lease;
                    this.isEmpty = string.IsNullOrWhiteSpace(text);
                    this.current = default!;
                    this.enumerator = default;
                    this.isEnumeratorSet = false;

                    this.reader.SetText(text);
                }

                public bool MoveNext()
                {
                    if (this.isEmpty)
                    {
                        this.current = default!;

                        return false;
                    }

                    if (!this.isEnumeratorSet)
                    {
                        this.enumerator = new TokenStreamEnumerable(this.analyzer.GetTokenStream(string.Empty, this.reader), this.buffer).GetEnumerator();
                        this.isEnumeratorSet = true;
                    }

                    while (this.enumerator.MoveNext())
                    {
                        var token = this.enumerator.Current;

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

                    this.lease?.Dispose();
                    this.lease = null;
                    this.isEmpty = default;
                }
            }
        }
    }
}
