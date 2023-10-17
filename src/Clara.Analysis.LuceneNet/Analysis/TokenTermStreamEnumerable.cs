using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct TokenTermStreamEnumerable : IEnumerable<TokenTerm>
    {
        private readonly TokenStream tokenStream;
        private readonly char[] chars;

        public TokenTermStreamEnumerable(TokenStream tokenStream, char[] chars)
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

        readonly IEnumerator<TokenTerm> IEnumerable<TokenTerm>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<TokenTerm>
        {
            private readonly TokenStream tokenStream;
            private readonly ICharTermAttribute charTermAttribute;
            private readonly IOffsetAttribute offsetAttribute;
            private readonly IPositionIncrementAttribute positionIncrementAttribute;
            private TokenTerm current;
            private Token token;
            private int position;
            private bool isStarted;

            internal Enumerator(TokenTermStreamEnumerable source)
            {
                this.tokenStream = source.tokenStream;
                this.charTermAttribute = source.tokenStream.GetAttribute<ICharTermAttribute>();
                this.offsetAttribute = source.tokenStream.GetAttribute<IOffsetAttribute>();
                this.positionIncrementAttribute = source.tokenStream.GetAttribute<IPositionIncrementAttribute>();
                this.current = default;
                this.token = new Token(source.chars, 0);
                this.position = default;
                this.isStarted = false;
            }

            public readonly TokenTerm Current
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
                    this.token.Set(this.charTermAttribute.Buffer.AsSpan(0, this.charTermAttribute.Length));
                    this.position += this.positionIncrementAttribute.PositionIncrement;
                    this.current = new TokenTerm(this.token, new Offset(this.offsetAttribute.StartOffset, this.offsetAttribute.EndOffset, this.position));

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
