using System;
using System.Collections;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    public readonly struct TokenStreamEnumerable : IEnumerable<string>
    {
        private readonly TokenStream tokenStream;
        private readonly IStringFactory stringFactory;

        public TokenStreamEnumerable(TokenStream tokenStream, IStringFactory stringFactory)
        {
            if (tokenStream is null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            this.tokenStream = tokenStream;
            this.stringFactory = stringFactory;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<string>
        {
            private readonly TokenStream tokenStream;
            private readonly IStringFactory stringFactory;
            private readonly ICharTermAttribute charTermAttribute;
            private bool isStarted;
            private string current;

            public Enumerator(TokenStreamEnumerable source)
            {
                this.tokenStream = source.tokenStream;
                this.stringFactory = source.stringFactory;
                this.charTermAttribute = source.tokenStream.GetAttribute<ICharTermAttribute>();
                this.isStarted = false;
                this.current = string.Empty;
            }

            public string Current
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
                        this.current = string.Empty;

                        return true;
                    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                    this.current = this.stringFactory.Create(this.charTermAttribute.Buffer.AsSpan(0, this.charTermAttribute.Length));
#else
                    this.current = this.stringFactory.Create(this.charTermAttribute.Buffer, 0, this.charTermAttribute.Length);
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
                this.current = string.Empty;
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
