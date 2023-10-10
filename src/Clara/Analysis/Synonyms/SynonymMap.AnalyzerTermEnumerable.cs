using System.Collections;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design")]
        public readonly record struct AnalyzerTermEnumerable : IEnumerable<AnalyzerTerm>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly SynonymMap synonymMap;
            private readonly string text;

            internal AnalyzerTermEnumerable(SynonymMap synonymMap, string text)
            {
                this.synonymMap = synonymMap;
                this.text = text;
            }

            public IEnumerator<AnalyzerTerm> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.synonymMap, this.text);

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
                private SynonymMap synonymMap = default!;
                private IAnalyzer analyzer = default!;
                private string text = default!;
                private bool isEmpty;
                private SynonymAnalyzerTermEnumerable.Enumerator synonymResultEnumerator;
                private ListSlim<string>.Enumerator replacementTokenEnumerator;
                private AnalyzerTerm current;
                private int state;

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

                public void Initialize(ObjectPoolLease<Enumerator> lease, SynonymMap synonymMap, string text)
                {
                    this.lease = lease;
                    this.synonymMap = synonymMap;
                    this.analyzer = synonymMap.analyzer;
                    this.text = text;
                    this.isEmpty = string.IsNullOrWhiteSpace(text);
                    this.synonymResultEnumerator = default;
                    this.replacementTokenEnumerator = default;
                    this.current = default;
                    this.state = 0;
                }

                public bool MoveNext()
                {
                    if (this.isEmpty)
                    {
                        this.current = default;

                        return false;
                    }

                    if (this.state == 0)
                    {
                        this.synonymResultEnumerator = new SynonymAnalyzerTermEnumerable(this.synonymMap, new PrimitiveEnumerable<AnalyzerTerm>(this.analyzer.GetTerms(this.text))).GetEnumerator();
                        this.state = 1;
                    }

                    if (this.state == 2)
                    {
                        if (this.replacementTokenEnumerator.MoveNext())
                        {
                            this.current = new AnalyzerTerm(this.synonymResultEnumerator.Current.Ordinal, new Token(this.replacementTokenEnumerator.Current));

                            return true;
                        }

                        this.replacementTokenEnumerator.Dispose();
                        this.replacementTokenEnumerator = default;
                        this.state = 1;
                    }

                    while (this.synonymResultEnumerator.MoveNext())
                    {
                        if (this.synonymResultEnumerator.Current.Node is TokenNode node)
                        {
                            this.replacementTokenEnumerator = node.ReplacementTokens.GetEnumerator();
                            this.state = 2;

                            if (this.replacementTokenEnumerator.MoveNext())
                            {
                                this.current = new AnalyzerTerm(this.synonymResultEnumerator.Current.Ordinal, new Token(this.replacementTokenEnumerator.Current));

                                return true;
                            }

                            this.replacementTokenEnumerator.Dispose();
                            this.replacementTokenEnumerator = default;
                            this.state = 1;
                        }
                        else if (this.synonymResultEnumerator.Current.Token is Token token)
                        {
                            this.current = new AnalyzerTerm(this.synonymResultEnumerator.Current.Ordinal, token);

                            return true;
                        }
                    }

                    this.current = default;

                    return false;
                }

                public void Reset()
                {
                    if (this.state == 2)
                    {
                        this.replacementTokenEnumerator.Dispose();
                        this.replacementTokenEnumerator = default;
                        this.state = 1;
                    }

                    if (this.state == 1)
                    {
                        this.synonymResultEnumerator.Dispose();
                        this.synonymResultEnumerator = default;
                        this.state = 0;
                    }

                    this.current = default;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.synonymMap = default!;
                    this.analyzer = default!;
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
