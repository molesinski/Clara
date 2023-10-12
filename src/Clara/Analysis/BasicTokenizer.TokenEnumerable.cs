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
                private char[]? additionalWordCharacters;
                private char[]? wordConnectingCharacters;
                private char[]? numberConnectingCharacters;
                private string text = default!;
                private bool isEmpty;
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
                    this.additionalWordCharacters = tokenizer.additionalWordCharacters;
                    this.wordConnectingCharacters = tokenizer.wordConnectingCharacters;
                    this.numberConnectingCharacters = tokenizer.numberConnectingCharacters;
                    this.text = text;
                    this.isEmpty = string.IsNullOrWhiteSpace(text);
                    this.startIndex = -1;
                    this.index = 0;
                }

                public bool MoveNext()
                {
                    if (this.isEmpty)
                    {
                        this.current.Clear();

                        return false;
                    }

                    while (this.index < this.text.Length)
                    {
                        var previousChar = ' ';
                        var currentChar = this.text[this.index];
                        var nextChar = ' ';
                        var hasToken = false;

                        if (this.index - 1 >= 0)
                        {
                            previousChar = this.text[this.index - 1];
                        }

                        if (this.index + 1 < this.text.Length)
                        {
                            nextChar = this.text[this.index + 1];
                        }

                        if (this.startIndex == -1)
                        {
                            if (this.IsWordOrNumber(currentChar))
                            {
                                this.startIndex = this.index;
                            }
                        }
                        else
                        {
                            if (this.IsWordOrNumber(currentChar))
                            {
                            }
                            else if (this.IsWordConnectingCharacter(currentChar) && this.IsWord(previousChar) && this.IsWord(nextChar))
                            {
                            }
                            else if (this.IsNumberDecimalSeparator(currentChar) && IsNumber(previousChar) && IsNumber(nextChar))
                            {
                            }
                            else
                            {
                                var count = this.index - this.startIndex;

                                if (count <= Token.MaximumLength)
                                {
                                    this.current.Set(this.text.AsSpan(this.startIndex, count));
                                    hasToken = true;
                                }

                                this.startIndex = -1;
                            }
                        }

                        this.index++;

                        if (hasToken)
                        {
                            return true;
                        }
                    }

                    if (this.startIndex != -1)
                    {
                        var hasToken = false;
                        var count = this.text.Length - this.startIndex;

                        if (count <= Token.MaximumLength)
                        {
                            this.current.Set(this.text.AsSpan(this.startIndex, count));
                            hasToken = true;
                        }

                        this.startIndex = -1;

                        if (hasToken)
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

                    this.additionalWordCharacters = default;
                    this.wordConnectingCharacters = default;
                    this.numberConnectingCharacters = default;
                    this.text = default!;
                    this.isEmpty = default;

                    var lease = this.lease;
                    this.lease = null;
                    lease?.Dispose();
                }

                private static bool IsNumber(char c)
                {
                    return char.IsDigit(c);
                }

                private bool IsWordOrNumber(char c)
                {
                    return this.IsWord(c) || IsNumber(c);
                }

                private bool IsWord(char c)
                {
                    if (char.IsLetter(c))
                    {
                        return true;
                    }

                    if (this.additionalWordCharacters is null)
                    {
                        return
                            c switch
                            {
                                '_' => true,
                                _ => false,
                            };
                    }
                    else
                    {
                        var chars = this.additionalWordCharacters;
                        var length = chars.Length;

                        for (var i = 0; i < length; i++)
                        {
                            if (c == chars[i])
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                }

                private bool IsWordConnectingCharacter(char c)
                {
                    if (this.wordConnectingCharacters is null)
                    {
                        return
                            c switch
                            {
                                '\'' => true,
                                '\u2019' => true,
                                '\uFF07' => true,
                                _ => false,
                            };
                    }
                    else
                    {
                        var chars = this.wordConnectingCharacters;
                        var length = chars.Length;

                        for (var i = 0; i < length; i++)
                        {
                            if (c == chars[i])
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                }

                private bool IsNumberDecimalSeparator(char c)
                {
                    if (this.numberConnectingCharacters is null)
                    {
                        return
                            c switch
                            {
                                '.' => true,
                                ',' => true,
                                _ => false,
                            };
                    }
                    else
                    {
                        var chars = this.numberConnectingCharacters;
                        var length = chars.Length;

                        for (var i = 0; i < length; i++)
                        {
                            if (c == chars[i])
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                }
            }
        }
    }
}
