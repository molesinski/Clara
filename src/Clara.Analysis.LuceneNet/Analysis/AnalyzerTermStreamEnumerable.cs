using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct AnalyzerTermStreamEnumerable : IEnumerable<AnalyzerTerm>
    {
        private readonly TokenStream tokenStream;
        private readonly char[] chars;

        public AnalyzerTermStreamEnumerable(TokenStream tokenStream, char[] chars)
        {
            if (tokenStream is null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            if (chars is null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            this.tokenStream = tokenStream;
            this.chars = chars;
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        readonly IEnumerator<AnalyzerTerm> IEnumerable<AnalyzerTerm>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<AnalyzerTerm>
        {
            private readonly TokenStream tokenStream;
            private readonly ICharTermAttribute charTermAttribute;
            private readonly IPositionIncrementAttribute positionIncrementAttribute;
            private Token token;
            private int ordinal;
            private AnalyzerTerm current;
            private bool isStarted;

            internal Enumerator(AnalyzerTermStreamEnumerable source)
            {
                this.tokenStream = source.tokenStream;
                this.charTermAttribute = source.tokenStream.GetAttribute<ICharTermAttribute>();
                this.positionIncrementAttribute = source.tokenStream.GetAttribute<IPositionIncrementAttribute>();
                this.token = new Token(source.chars, 0);
                this.ordinal = default;
                this.current = default;
                this.isStarted = false;
            }

            public readonly AnalyzerTerm Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public bool MoveNext()
            {
                if (!this.isStarted)
                {
                    this.tokenStream.Reset();
                    this.isStarted = true;
                }

                if (this.tokenStream.IncrementToken())
                {
                    this.ordinal += this.positionIncrementAttribute.PositionIncrement;
                    this.token.Set(this.charTermAttribute.Buffer.AsSpan(0, this.charTermAttribute.Length));
                    this.current = new AnalyzerTerm(this.ordinal, this.token);

                    return true;
                }

                this.current = default;

                return false;
            }

            public void Reset()
            {
                if (this.isStarted)
                {
                    this.tokenStream.End();
                }

                this.isStarted = false;
                this.current = default;
            }

            public void Dispose()
            {
                this.Reset();

                this.tokenStream.Dispose();
            }
        }
    }
}
