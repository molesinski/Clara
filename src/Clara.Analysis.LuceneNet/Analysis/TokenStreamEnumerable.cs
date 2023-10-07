using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct TokenStreamEnumerable : IEnumerable<Token>
    {
        private readonly TokenStream tokenStream;
        private readonly char[] chars;

        public TokenStreamEnumerable(TokenStream tokenStream, char[] chars)
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

        readonly IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<Token>
        {
            private readonly TokenStream tokenStream;
            private readonly ICharTermAttribute charTermAttribute;
            private readonly char[] chars;
            private bool isStarted;
            private Token current;

            internal Enumerator(TokenStreamEnumerable source)
            {
                this.tokenStream = source.tokenStream;
                this.charTermAttribute = source.tokenStream.GetAttribute<ICharTermAttribute>();
                this.chars = source.chars;
                this.isStarted = false;
                this.current = default;
            }

            public readonly Token Current
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
                    if (this.charTermAttribute.Length == 0)
                    {
                        this.current = default;

                        return true;
                    }

                    var length = this.charTermAttribute.Length;

                    Array.Copy(this.charTermAttribute.Buffer, this.chars, length);

                    this.current = new Token(this.chars, length);

                    return true;
                }

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
            }
        }
    }
}
