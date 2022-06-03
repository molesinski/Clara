using System;
using System.Collections;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    public readonly struct TokenStreamEnumerable : IEnumerable<Token>
    {
        private readonly TokenStream tokenStream;

        public TokenStreamEnumerable(TokenStream tokenStream)
        {
            if (tokenStream is null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            this.tokenStream = tokenStream;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<Token>
        {
            private readonly TokenStream tokenStream;
            private readonly ICharTermAttribute charTermAttribute;
            private bool isStarted;
            private Token current;

            public Enumerator(TokenStreamEnumerable source)
            {
                this.tokenStream = source.tokenStream;
                this.charTermAttribute = source.tokenStream.GetAttribute<ICharTermAttribute>();
                this.isStarted = false;
                this.current = default;
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
                    return this.Current;
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

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                    this.current = new Token(new string(this.charTermAttribute.Buffer.AsSpan(0, this.charTermAttribute.Length)));
#else
                    this.current = new Token(new string(this.charTermAttribute.Buffer, 0, this.charTermAttribute.Length));
#endif

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
                if (this.isStarted)
                {
                    this.tokenStream.End();
                }
            }
        }
    }
}
