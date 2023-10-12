using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed partial class BasicTokenizer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design")]
        public readonly record struct TokenEnumerable : IEnumerable<Token>
        {
            private static readonly ObjectPool<Enumerator> Pool = new(() => new());

            private readonly BasicTokenizer tokenizer;
            private readonly string text;

            internal TokenEnumerable(BasicTokenizer tokenizer, string text)
            {
                this.tokenizer = tokenizer;
                this.text = text;
            }

            public IEnumerator<Token> GetEnumerator()
            {
                var lease = Pool.Lease();

                lease.Instance.Initialize(lease, this.tokenizer, this.text);

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
                private int startIndex;
                private int index;

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

                public void Initialize(ObjectPoolLease<Enumerator> lease, BasicTokenizer tokenizer, string text)
                {
                    this.lease = lease;
                    this.text = text;
                    this.startIndex = -1;
                    this.index = 0;
                }

                public bool MoveNext()
                {
                    var hasCurrent = false;

                    if (this.index < this.text.Length)
                    {
                        var pp = ' ';
                        var p = ' ';
                        var c = this.text[this.index];
                        var n = ' ';
                        var nn = ' ';

                        if (this.index - 1 >= 0)
                        {
                            p = this.text[this.index - 1];

                            if (this.index - 2 >= 0)
                            {
                                pp = this.text[this.index - 2];
                            }
                        }

                        if (this.index + 1 < this.text.Length)
                        {
                            n = this.text[this.index + 1];

                            if (this.index + 2 < this.text.Length)
                            {
                                nn = this.text[this.index + 2];
                            }
                        }

                        while (this.index < this.text.Length)
                        {
                            if (this.startIndex == -1)
                            {
                                if (char.IsLetterOrDigit(c) || c == '_')
                                {
                                    this.startIndex = this.index;
                                }
                            }
                            else
                            {
                                if (char.IsLetterOrDigit(c) || c == '_')
                                {
                                }
                                else if (IsConnector(pp, p, c, n, nn))
                                {
                                }
                                else
                                {
                                    var count = this.index - this.startIndex;

                                    if (count <= Token.MaximumLength && !IsUnderscores(this.text, this.startIndex, count))
                                    {
                                        this.current.Set(this.text.AsSpan(this.startIndex, count));
                                        hasCurrent = true;
                                    }

                                    this.startIndex = -1;
                                }
                            }

                            this.index++;

                            if (hasCurrent)
                            {
                                return true;
                            }

                            pp = p;
                            p = c;
                            c = n;
                            n = nn;
                            nn = ' ';

                            if (this.index + 2 < this.text.Length)
                            {
                                nn = this.text[this.index + 2];
                            }
                        }
                    }

                    if (this.startIndex != -1)
                    {
                        var count = this.text.Length - this.startIndex;

                        if (count <= Token.MaximumLength && !IsUnderscores(this.text, this.startIndex, count))
                        {
                            this.current.Set(this.text.AsSpan(this.startIndex, count));
                            hasCurrent = true;
                        }

                        this.startIndex = -1;

                        if (hasCurrent)
                        {
                            return true;
                        }
                    }

                    this.current.Clear();
                    return false;
                }

                public void Reset()
                {
                    this.current.Clear();
                    this.startIndex = -1;
                    this.index = 0;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.text = default!;

                    var lease = this.lease;
                    this.lease = null;
                    lease?.Dispose();
                }

                private static bool IsConnector(char pp, char p, char c, char n, char nn)
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

                private static bool IsUnderscores(string text, int index, int count)
                {
                    for (var i = index; i < index + count; i++)
                    {
                        if (text[i] != '_')
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
