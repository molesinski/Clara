using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct ReadOnlyTokenTermSourceEnumerable : IEnumerable<Token>
    {
        private readonly TokenStream tokenStream;

        public ReadOnlyTokenTermSourceEnumerable(TokenStream tokenStream)
        {
            if (tokenStream is null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            this.tokenStream = tokenStream;
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
            private readonly TokenStream tokenTermSource;
            private readonly ICharTermAttribute charTermAttribute;
            private bool isStarted;
            private Token current;

            internal Enumerator(ReadOnlyTokenTermSourceEnumerable source)
            {
                this.tokenTermSource = source.tokenStream;
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
                    return this.current;
                }
            }

            public bool MoveNext()
            {
                if (!this.isStarted)
                {
                    this.tokenTermSource.Reset();
                    this.isStarted = true;
                }

                if (this.tokenTermSource.IncrementToken())
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
                    this.tokenTermSource.End();
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
