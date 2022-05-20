using System;
using System.Collections;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    public sealed class TokenStreamEnumerable : IEnumerable<string>
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

        public IEnumerator<string> GetEnumerator()
        {
            return new Enumerator(this.tokenStream);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<string>
        {
            private readonly TokenStream tokenStream;
            private readonly ICharTermAttribute charTermAttribute;
            private bool isStarted;
            private string current;

            public Enumerator(TokenStream tokenStream)
            {
                this.tokenStream = tokenStream;
                this.charTermAttribute = tokenStream.GetAttribute<ICharTermAttribute>();
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
                    this.current = new string(this.charTermAttribute.Buffer, 0, this.charTermAttribute.Length);

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
