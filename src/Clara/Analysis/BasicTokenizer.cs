using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class BasicTokenizer : ITokenizer
    {
        private readonly ObjectPool<TokenEnumerable> enumerablePool;
        private readonly char[]? additionalWordCharacters;
        private readonly char[]? wordConnectingCharacters;
        private readonly char[]? numberConnectingCharacters;

        public BasicTokenizer(
            IEnumerable<char>? additionalWordCharacters = null,
            IEnumerable<char>? wordConnectingCharacters = null,
            IEnumerable<char>? numberConnectingCharacters = null)
        {
            this.enumerablePool = new(() => new(this));
            this.additionalWordCharacters = additionalWordCharacters?.ToArray();
            this.wordConnectingCharacters = wordConnectingCharacters?.ToArray();
            this.numberConnectingCharacters = numberConnectingCharacters?.ToArray();
        }

        public IDisposableEnumerable<Token> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return DisposableEnumerable<Token>.Empty;
            }

            var enumerable = this.enumerablePool.Lease();

            enumerable.Instance.Initialize(text, enumerable);

            return enumerable.Instance;
        }

        private sealed class TokenEnumerable : IDisposableEnumerable<Token>
        {
            private readonly Enumerator enumerator;
            private ObjectPoolLease<TokenEnumerable> lease;
            private bool isDisposed;

            public TokenEnumerable(BasicTokenizer tokenizer)
            {
                this.enumerator = new Enumerator(tokenizer);
                this.lease = default;
                this.isDisposed = true;
            }

            public void Initialize(string text, ObjectPoolLease<TokenEnumerable> lease)
            {
                if (!this.isDisposed)
                {
                    throw new InvalidOperationException("Current object instance is already initialized.");
                }

                this.enumerator.Initialize(text);
                this.lease = lease;
                this.isDisposed = false;
            }

            public Enumerator GetEnumerator()
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.enumerator;
            }

            IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            void IDisposable.Dispose()
            {
                if (!this.isDisposed)
                {
                    this.enumerator.Dispose();
                    this.lease.Dispose();
                    this.lease = default;
                    this.isDisposed = true;
                }
            }

            public sealed class Enumerator : IEnumerator<Token>
            {
                public static readonly IEnumerable<char> DefaultAdditionalWordCharacters = new[] { '_' };
                public static readonly IEnumerable<char> DefaultWordConnectingCharacters = new[] { '-', '\'', '\u2019', '\uFF07' };
                public static readonly IEnumerable<char> DefaultNumberConnectingCharacters = new[] { '.', ',' };

                private readonly char[]? additionalWordCharacters;
                private readonly char[]? wordConnectingCharacters;
                private readonly char[]? numberConnectingCharacters;
                private string text;
                private Token current;
                private int startIndex;
                private int index;

                public Enumerator(BasicTokenizer tokenizer)
                {
                    this.additionalWordCharacters = tokenizer.additionalWordCharacters;
                    this.wordConnectingCharacters = tokenizer.wordConnectingCharacters;
                    this.numberConnectingCharacters = tokenizer.numberConnectingCharacters;
                    this.text = default!;
                    this.current = new Token(new char[Token.MaximumLength], 0);
                    this.startIndex = -1;
                    this.index = 0;
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
                        return this.current;
                    }
                }

                public void Initialize(string text)
                {
                    this.text = text;
                }

                public bool MoveNext()
                {
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

                    this.text = default!;
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
                                '-' => true,
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
