using System.Collections;
using Clara.Utils;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardAnalyzer : IAnalyzer
    {
        private static readonly IEnumerable<AnalyzerTerm> Empty = new TokenEnumerable(string.Empty);

        public ITokenizer Tokenizer
        {
            get
            {
                return LuceneStandardTokenizer.Instance;
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
                return Empty;
            }

            return new TokenEnumerable(text);
        }

        internal readonly struct TokenEnumerable : IEnumerable<AnalyzerTerm>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly string text;

            internal TokenEnumerable(string text)
            {
                this.text = text;
            }

            public IEnumerator<AnalyzerTerm> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.text);

                return lease.Instance;
            }

            IEnumerator<AnalyzerTerm> IEnumerable<AnalyzerTerm>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Dispose returns object to pool for reuse")]
            private sealed class Enumerator : IEnumerator<AnalyzerTerm>
            {
                private readonly ReusableStringReader reader;
                private readonly Lucene.Net.Analysis.Analyzer analyzer;
                private readonly char[] chars;
                private ObjectPoolLease<Enumerator>? lease;
                private bool isEmpty;
                private AnalyzerTerm current;
                private AnalyzerTermStreamEnumerable.Enumerator enumerator;
                private bool isEnumeratorSet;

                public Enumerator()
                {
                    this.reader = new ReusableStringReader();
                    this.analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
                    this.chars = new char[Token.MaximumLength];
                }

                public AnalyzerTerm Current
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
                    this.current = default;
                    this.enumerator = default;
                    this.isEnumeratorSet = false;

                    this.reader.SetText(text);
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
                        this.enumerator = new AnalyzerTermStreamEnumerable(this.analyzer.GetTokenStream(string.Empty, this.reader), this.chars).GetEnumerator();
                        this.isEnumeratorSet = true;
                    }

                    while (this.enumerator.MoveNext())
                    {
                        this.current = this.enumerator.Current;

                        return true;
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
                    this.current = default;
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
