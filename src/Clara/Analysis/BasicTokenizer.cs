using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class BasicTokenizer : ITokenizer
    {
        public static readonly IEnumerable<char> DefaultWordConnectingCharacters = new[] { '-', '\'', '\u2019', '\uFF07' };
        public static readonly IEnumerable<char> DefaultNumberDecimalSeparators = new[] { '.', ',' };

        private static readonly ObjectPool<char[]> CharsPool = new(() => new char[Token.MaximumLength]);

        private readonly char[]? wordConnectingCharacters;
        private readonly char[]? numberDecimalSeparators;

        public BasicTokenizer(
            IEnumerable<char>? wordConnectingCharacters = null,
            IEnumerable<char>? numberDecimalSeparators = null)
        {
            this.wordConnectingCharacters = wordConnectingCharacters?.ToArray();
            this.numberDecimalSeparators = numberDecimalSeparators?.ToArray();
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

            var length = text.Length;
            var previous = ' ';
            var current = text[0];
            var index = -1;

            using var chars = CharsPool.Lease();

            for (var i = 0; i < length; i++)
            {
                var next = ' ';

                if (i + 1 < length)
                {
                    next = text[i + 1];
                }

                if (index == -1)
                {
                    if (IsWordOrNumber(current))
                    {
                        index = i;
                    }
                }
                else
                {
                    if (IsWordOrNumber(current))
                    {
                    }
                    else if (this.IsWordConnectingCharacter(current) && IsWord(previous) && IsWord(next))
                    {
                    }
                    else if (this.IsNumberDecimalSeparator(current) && IsNumber(previous) && IsNumber(next))
                    {
                    }
                    else
                    {
                        var count = i - index;

                        if (count <= Token.MaximumLength)
                        {
                            yield return ToToken(text, index, count, chars.Instance);
                        }

                        index = -1;
                    }
                }

                previous = current;
                current = next;
            }

            if (index != -1)
            {
                var count = length - index;

                if (count <= Token.MaximumLength)
                {
                    yield return ToToken(text, index, count, chars.Instance);
                }
            }
        }

        private static Token ToToken(string text, int index, int count, char[] chars)
        {
            text.CopyTo(index, chars, 0, count);

            return new Token(chars, count);
        }

        private static bool IsWordOrNumber(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        private static bool IsWord(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        private static bool IsNumber(char c)
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
                for (var i = 0; i < this.wordConnectingCharacters.Length; i++)
                {
                    if (c == this.wordConnectingCharacters[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool IsNumberDecimalSeparator(char c)
        {
            if (this.numberDecimalSeparators is null)
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
                for (var i = 0; i < this.numberDecimalSeparators.Length; i++)
                {
                    if (c == this.numberDecimalSeparators[i])
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
