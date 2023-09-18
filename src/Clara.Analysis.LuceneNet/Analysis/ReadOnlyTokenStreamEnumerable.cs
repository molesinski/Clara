using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct ReadOnlyTokenStreamEnumerable : IEnumerable<Token>
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        private readonly TokenStream tokenStream;

        public ReadOnlyTokenStreamEnumerable(TokenStream tokenStream)
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

            internal Enumerator(ReadOnlyTokenStreamEnumerable source)
            {
                this.tokenStream = source.tokenStream;
                this.charTermAttribute = source.tokenStream.GetAttribute<ICharTermAttribute>();
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

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
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

                this.isStarted = false;
                this.current = default;
            }
        }
    }
}
