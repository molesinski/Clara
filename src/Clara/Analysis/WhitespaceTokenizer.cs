using System.Collections;

namespace Clara.Analysis
{
    public sealed class WhitespaceTokenizer : ITokenizer
    {
        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource();
        }

        public bool Equals(ITokenizer? other)
        {
            return other is WhitespaceTokenizer;
        }

        private sealed class TokenTermSource : ITokenTermSource, IEnumerable<TokenTerm>, IEnumerator<TokenTerm>
        {
            private string text = string.Empty;
            private TokenTerm current;
            private Token token = new(new char[Token.MaximumLength], 0);
            private int lastPosition;
            private int lastIndex;

            TokenTerm IEnumerator<TokenTerm>.Current
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

            public IEnumerable<TokenTerm> GetTerms(string text)
            {
                if (text is null)
                {
                    throw new ArgumentNullException(nameof(text));
                }

                this.text = text;

                ((IEnumerator)this).Reset();

                return this;
            }

            IEnumerator<TokenTerm> IEnumerable<TokenTerm>.GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            bool IEnumerator.MoveNext()
            {
                var hasCurrent = false;
                var start = -1;
                var position = this.lastPosition;
                var i = this.lastIndex;

                if (i < this.text.Length)
                {
                    while (i < this.text.Length)
                    {
                        if (start == -1)
                        {
                            if (!char.IsWhiteSpace(this.text[i]))
                            {
                                start = i;
                            }
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(this.text[i]))
                            {
                            }
                            else
                            {
                                var span = this.text.AsSpan(start, i - start);

                                if (span.Length <= Token.MaximumLength)
                                {
                                    this.token.Set(span);
                                    this.current = new TokenTerm(this.token, new TokenPosition(position++));

                                    hasCurrent = true;
                                }

                                start = -1;
                            }
                        }

                        i++;

                        if (hasCurrent)
                        {
                            this.lastPosition = position;
                            this.lastIndex = i;

                            return true;
                        }
                    }
                }

                if (start != -1)
                {
                    var span = this.text.AsSpan(start, this.text.Length - start);

                    if (span.Length <= Token.MaximumLength)
                    {
                        this.token.Set(span);
                        this.current = new TokenTerm(this.token, new TokenPosition(position++));

                        this.lastPosition = position;
                        this.lastIndex = i;

                        return true;
                    }
                }

                this.current = default;
                return false;
            }

            void IEnumerator.Reset()
            {
                this.current = default;
                this.lastPosition = 0;
                this.lastIndex = 0;
            }

            void IDisposable.Dispose()
            {
                ((IEnumerator)this).Reset();
            }
        }
    }
}
