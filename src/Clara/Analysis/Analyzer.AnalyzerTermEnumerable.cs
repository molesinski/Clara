using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed partial class Analyzer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design")]
        public readonly record struct AnalyzerTermEnumerable : IEnumerable<AnalyzerTerm>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly Analyzer analyzer;
            private readonly string text;

            internal AnalyzerTermEnumerable(Analyzer analyzer, string text)
            {
                this.analyzer = analyzer;
                this.text = text;
            }

            public IEnumerator<AnalyzerTerm> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.analyzer, this.text);

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

            private sealed class Enumerator : IEnumerator<AnalyzerTerm>
            {
                private ObjectPoolLease<Enumerator>? lease;
                private ITokenizer tokenizer = default!;
                private TokenFilterDelegate pipeline = default!;
                private string text = default!;
                private bool isEmpty;
                private int position;
                private AnalyzerTerm current;
                private IEnumerator<Token>? enumerator;

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

                public void Initialize(ObjectPoolLease<Enumerator> lease, Analyzer analyzer, string text)
                {
                    this.lease = lease;
                    this.tokenizer = analyzer.tokenizer;
                    this.pipeline = analyzer.pipeline;
                    this.text = text;
                    this.isEmpty = string.IsNullOrWhiteSpace(text);
                    this.position = default;
                    this.current = default!;
                    this.enumerator = default;
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
                        var position = ++this.position;
                        var token = this.enumerator.Current;

                        token = this.pipeline(token);

                        if (!token.IsEmpty)
                        {
                            this.current = new AnalyzerTerm(position, token);

                            return true;
                        }
                    }

                    this.current = default!;

                    return false;
                }

                public void Reset()
                {
                    this.enumerator?.Dispose();
                    this.enumerator = default;
                    this.position = default;
                    this.current = default!;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.tokenizer = default!;
                    this.pipeline = default!;
                    this.text = default!;
                    this.isEmpty = default;

                    var lease = this.lease;
                    this.lease = null;
                    lease?.Dispose();
                }
            }
        }
    }
}
