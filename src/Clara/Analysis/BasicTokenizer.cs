using Clara.Storage;

namespace Clara.Analysis
{
    public sealed class BasicTokenizer : ITokenizer
    {
        public static readonly IEnumerable<char> DefaultAdditionalWordCharacters = new[] { '_' };
        public static readonly IEnumerable<char> DefaultWordConnectingCharacters = new[] { '-', '\'', '\u2019', '\uFF07' };
        public static readonly IEnumerable<char> DefaultNumberConnectingCharacters = new[] { '.', ',' };

        private readonly char[]? additionalWordCharacters;
        private readonly char[]? wordConnectingCharacters;
        private readonly char[]? numberConnectingCharacters;

        public BasicTokenizer(
            IEnumerable<char>? additionalWordCharacters = null,
            IEnumerable<char>? wordConnectingCharacters = null,
            IEnumerable<char>? numberConnectingCharacters = null)
        {
            this.additionalWordCharacters = additionalWordCharacters?.ToArray();
            this.wordConnectingCharacters = wordConnectingCharacters?.ToArray();
            this.numberConnectingCharacters = numberConnectingCharacters?.ToArray();
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                yield break;
            }

            var previous = ' ';
            var current = text[0];
            var index = -1;

            using var buffer = SharedObjectPools.TokenBuffers.Lease();

            for (var i = 0; i < text.Length; i++)
            {
                var next = ' ';

                if (i + 1 < text.Length)
                {
                    next = text[i + 1];
                }

                if (index == -1)
                {
                    if (this.IsWordOrNumber(current))
                    {
                        index = i;
                    }
                }
                else
                {
                    if (this.IsWordOrNumber(current))
                    {
                    }
                    else if (this.IsWordConnectingCharacter(current) && this.IsWord(previous) && this.IsWord(next))
                    {
                    }
                    else if (this.IsNumberDecimalSeparator(current) && this.IsNumber(previous) && this.IsNumber(next))
                    {
                    }
                    else
                    {
                        var count = i - index;

                        if (count <= Token.MaximumLength)
                        {
                            yield return ToToken(text, index, count, buffer.Instance);
                        }

                        index = -1;
                    }
                }

                previous = current;
                current = next;
            }

            if (index != -1)
            {
                var count = text.Length - index;

                if (count <= Token.MaximumLength)
                {
                    yield return ToToken(text, index, count, buffer.Instance);
                }
            }
        }

        private static Token ToToken(string text, int index, int count, char[] chars)
        {
            text.CopyTo(index, chars, 0, count);

            return new Token(chars, count);
        }

        private bool IsWordOrNumber(char c)
        {
            return this.IsWord(c) || this.IsNumber(c);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By design")]
        private bool IsNumber(char c)
        {
            return char.IsDigit(c);
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
