using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Clara.Analysis
{
    public sealed class BasicTokenizer : ITokenizer
    {
        private static readonly ArrayPool<char> CharPool = ArrayPool<char>.Shared;

        private readonly int maximumTokenLength;
        private readonly char[] additionalWordConnectingCharacters;
        private readonly char numberDecimalSeparator;

        public BasicTokenizer(
            int maximumTokenLength = Token.MaximumLength,
            IEnumerable<char>? additionalWordConnectingCharacters = null,
            char numberDecimalSeparator = '.')
        {
            if (maximumTokenLength < 0 || maximumTokenLength > Token.MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumTokenLength));
            }

            this.maximumTokenLength = maximumTokenLength;
            this.additionalWordConnectingCharacters = additionalWordConnectingCharacters?.ToArray() ?? Array.Empty<char>();
            this.numberDecimalSeparator = numberDecimalSeparator;
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

            var textLength = text.Length;
            var previous = ' ';
            var current = text[0];
            var start = -1;

            var chars = CharPool.Rent(this.maximumTokenLength * 2);

            try
            {
                for (var i = 0; i < textLength; i++)
                {
                    var next = ' ';

                    if (i + 1 < textLength)
                    {
                        next = text[i + 1];
                    }

                    if (start == -1)
                    {
                        if (IsWordOrNumber(current))
                        {
                            start = i;
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
                            var length = i - start;

                            if (length <= this.maximumTokenLength)
                            {
                                var token = ToToken(text, start, length, chars);

                                yield return token;
                            }

                            start = -1;
                        }
                    }

                    previous = current;
                    current = next;
                }

                if (start != -1)
                {
                    var length = textLength - start;

                    if (length <= this.maximumTokenLength)
                    {
                        var token = ToToken(text, start, length, chars);

                        yield return token;
                    }
                }
            }
            finally
            {
                CharPool.Return(chars);
            }
        }

        private static Token ToToken(string text, int start, int length, char[] chars)
        {
            for (var i = 0; i < length; i++)
            {
                chars[i] = text[start + i];
            }

            return new Token(chars, length);
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
            var result =
                c switch
                {
                    '-' => true,
                    '\'' => true,
                    '\u2019' => true,
                    '\uFF07' => true,
                    _ => false,
                };

            if (!result)
            {
                for (var i = 0; i < this.additionalWordConnectingCharacters.Length; i++)
                {
                    if (c == this.additionalWordConnectingCharacters[i])
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private bool IsNumberDecimalSeparator(char c)
        {
            if (c == this.numberDecimalSeparator)
            {
                return true;
            }

            return false;
        }
    }
}
