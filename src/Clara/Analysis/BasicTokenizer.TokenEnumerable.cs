using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed partial class BasicTokenizer
    {
        internal readonly struct TokenEnumerable : IEnumerable<Token>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly string text;

            internal TokenEnumerable(string text)
            {
                this.text = text;
            }

            public IEnumerator<Token> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.text);

                return lease.Instance;
            }

            IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private sealed class Enumerator : IEnumerator<Token>
            {
                private ObjectPoolLease<Enumerator>? lease;
                private string text = default!;
                private Token current = new(new char[Token.MaximumLength], 0);
                private int lastIndex;

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
                        return this.current;
                    }
                }

                public void Initialize(ObjectPoolLease<Enumerator> lease, string text)
                {
                    this.lease = lease;
                    this.text = text;
                    this.lastIndex = 0;
                }

                public bool MoveNext()
                {
                    var hasCurrent = false;
                    var start = -1;
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
                                        this.current.Set(span);

                                        hasCurrent = true;
                                    }

                                    start = -1;
                                }
                            }

                            i++;

                            if (hasCurrent)
                            {
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
                            this.current.Set(span);
                            this.lastIndex = i;

                            return true;
                        }
                    }

                    this.current.Clear();
                    return false;
                }

                public void Reset()
                {
                    this.current.Clear();
                    this.lastIndex = 0;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.text = default!;

                    var lease = this.lease;
                    this.lease = null;
                    lease?.Dispose();
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
}
