﻿using System.Collections;
using Clara.Utils;
using Lucene.Net.Analysis.En;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishAnalyzer : IAnalyzer
    {
        private readonly IEnumerable<AnalyzerTerm> emptyEnumerable = new TokenEnumerable(string.Empty);

        public ITokenizer Tokenizer
        {
            get
            {
                return LuceneStandardTokenizer.Instance;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By design")]
        public TokenEnumerable GetTerms(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new TokenEnumerable(text);
        }

        IEnumerable<AnalyzerTerm> IAnalyzer.GetTerms(string text)
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
        public readonly record struct TokenEnumerable : IEnumerable<AnalyzerTerm>
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

            private sealed class Enumerator : IEnumerator<AnalyzerTerm>
            {
#pragma warning disable CA2213 // Disposable fields should be disposed
                private readonly ReusableStringReader reader;
                private readonly Lucene.Net.Analysis.Analyzer analyzer;
#pragma warning restore CA2213 // Disposable fields should be disposed
                private readonly char[] chars;
                private ObjectPoolLease<Enumerator>? lease;
                private bool isEmpty;
                private AnalyzerTerm current;
                private AnalyzerTermStreamEnumerable.Enumerator enumerator;
                private bool isEnumeratorSet;

                public Enumerator()
                {
                    this.reader = new ReusableStringReader();
                    this.analyzer = new EnglishAnalyzer(LuceneVersion.LUCENE_48);
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
