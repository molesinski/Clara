using System.Collections;

namespace Clara.Analysis
{
    public sealed class StandardTokenizer : ITokenizer
    {
        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource();
        }

        public bool Equals(ITokenizer? other)
        {
            return other is StandardTokenizer;
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
                    var pp = ' ';
                    var p = ' ';
                    var c = this.text[i];
                    var n = ' ';
                    var nn = ' ';

                    if (i - 2 >= 0)
                    {
                        pp = this.text[i - 2];
                        p = this.text[i - 1];
                    }
                    else if (i - 1 >= 0)
                    {
                        p = this.text[i - 1];
                    }

                    if (i + 2 < this.text.Length)
                    {
                        nn = this.text[i + 2];
                        n = this.text[i + 1];
                    }
                    else if (i + 1 < this.text.Length)
                    {
                        n = this.text[i + 1];
                    }

                    while (i < this.text.Length)
                    {
                        if (start == -1)
                        {
                            if (char.IsLetterOrDigit(c) || c == '_')
                            {
                                start = i;
                            }
                        }
                        else
                        {
                            if (char.IsLetterOrDigit(c) || c == '_')
                            {
                            }
                            else if (IsConnected(pp, p, c, n, nn))
                            {
                            }
                            else
                            {
                                var span = this.text.AsSpan(start, i - start);

                                if (span.Length <= Token.MaximumLength && !IsUnderscores(span))
                                {
                                    this.token.Set(span);
                                    this.current = new TokenTerm(this.token, new Position(position++));

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

                        pp = p;
                        p = c;
                        c = n;
                        n = nn;
                        nn = ' ';

                        if (i + 2 < this.text.Length)
                        {
                            nn = this.text[i + 2];
                        }
                    }
                }

                if (start != -1)
                {
                    var span = this.text.AsSpan(start, this.text.Length - start);

                    if (span.Length <= Token.MaximumLength && !IsUnderscores(span))
                    {
                        this.token.Set(span);
                        this.current = new TokenTerm(this.token, new Position(position++));

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

            private static bool IsConnected(char pp, char p, char c, char n, char nn)
            {
                var result = false;

                if (char.IsLetter(p) && char.IsLetter(n))
                {
                    result =
                        c switch
                        {
                            '\'' => true,
                            '\u2019' => true,
                            '\uFF07' => true,
                            _ => false,
                        };

                    if (!result)
                    {
                        if (c == '.' && ((pp == '.' && (nn == '.' || !char.IsLetterOrDigit(nn))) || (nn == '.' && (pp == '.' || !char.IsLetterOrDigit(pp)))))
                        {
                            result = true;
                        }
                    }
                }
                else if (char.IsDigit(p) && char.IsDigit(n))
                {
                    result =
                        c switch
                        {
                            '.' => true,
                            ',' => true,
                            _ => false,
                        };
                }

                return result;
            }

            private static bool IsUnderscores(ReadOnlySpan<char> span)
            {
                for (var i = 0; i < span.Length; i++)
                {
                    if (span[i] != '_')
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
