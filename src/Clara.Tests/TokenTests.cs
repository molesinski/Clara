using Clara.Analysis;
using Xunit;

namespace Clara.Tests
{
    public class TokenTests
    {
        [Theory]
        [InlineData("", "", "")]
        [InlineData("aaa", "", "aaa")]
        [InlineData("", "bbb", "bbb")]
        [InlineData("aaa", "bbb", "aaabbb")]
        public void Append(string input, string chars, object expected)
        {
            if (expected is Type exceptionType)
            {
                Assert.Throws(
                    exceptionType,
                    () =>
                    {
                        var token = new Token(new char[Token.MaximumLength], 0);
                        token.Set(input);
                        token.Append(chars);
                    });
            }
            else if (expected is string expectedString)
            {
                var token = new Token(new char[Token.MaximumLength], 0);
                token.Set(input);
                token.Append(chars);

                Assert.Equal(expectedString, token.ToString());
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, "", typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, "", "")]
        [InlineData("aaa", 0, "", "")]
        [InlineData("aaa", 1, "", "a")]
        [InlineData("aaa", 2, "", "aa")]
        [InlineData("aaa", 3, "", "aaa")]
        [InlineData("", 0, "bbb", "bbb")]
        [InlineData("aaa", 3, "bbb", "aaabbb")]
        public void Write(string input, int startIndex, string chars, object expected)
        {
            if (expected is Type exceptionType)
            {
                Assert.Throws(
                    exceptionType,
                    () =>
                    {
                        var token = new Token(new char[Token.MaximumLength], 0);
                        token.Set(input);
                        token.Write(startIndex, chars);
                    });
            }
            else if (expected is string expectedString)
            {
                var token = new Token(new char[Token.MaximumLength], 0);
                token.Set(input);
                token.Write(startIndex, chars);

                Assert.Equal(expected, token.ToString());
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, "", typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, "", "")]
        [InlineData("aaa", 0, "", "aaa")]
        [InlineData("aaa", 1, "", "aaa")]
        [InlineData("aaa", 3, "", "aaa")]
        [InlineData("", 0, "bbb", "bbb")]
        [InlineData("aaa", 3, "bbb", "aaabbb")]
        [InlineData("aaa", 1, "bbb", "abbbaa")]
        public void Insert(string input, int startIndex, string chars, object expected)
        {
            if (expected is Type exceptionType)
            {
                Assert.Throws(
                    exceptionType,
                    () =>
                    {
                        var token = new Token(new char[Token.MaximumLength], 0);
                        token.Set(input);
                        token.Insert(startIndex, chars);
                    });
            }
            else if (expected is string expectedString)
            {
                var token = new Token(new char[Token.MaximumLength], 0);
                token.Set(input);
                token.Insert(startIndex, chars);

                Assert.Equal(expected, token.ToString());
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, 0, typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, 1, typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, 0, "")]
        [InlineData("abc", 0, 3, "")]
        [InlineData("abc", 0, 1, "bc")]
        [InlineData("abc", 1, 2, "a")]
        [InlineData("abc", 1, 1, "ac")]
        [InlineData("abcde", 1, 3, "ae")]
        [InlineData("abc", 1, 0, "abc")]
        [InlineData("abc", 2, 1, "ab")]
        [InlineData("abc", 3, 0, "abc")]
        public void Delete(string input, int startIndex, int count, object expected)
        {
            if (expected is Type exceptionType)
            {
                Assert.Throws(
                    exceptionType,
                    () =>
                    {
                        var token = new Token(new char[Token.MaximumLength], 0);
                        token.Set(input);
                        token.Delete(startIndex, count);
                    });
            }
            else if (expected is string expectedString)
            {
                var token = new Token(new char[Token.MaximumLength], 0);
                token.Set(input);
                token.Delete(startIndex, count);

                Assert.Equal(expectedString, token.ToString());
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, "")]
        [InlineData("aaa", 0, "")]
        [InlineData("aaa", 1, "a")]
        [InlineData("aaa", 2, "aa")]
        [InlineData("aaa", 3, "aaa")]
        public void Remove(string input, int startIndex, object expected)
        {
            if (expected is Type exceptionType)
            {
                Assert.Throws(
                    exceptionType,
                    () =>
                    {
                        var token = new Token(new char[Token.MaximumLength], 0);
                        token.Set(input);
                        token.Remove(startIndex);
                    });
            }
            else if (expected is string expectedString)
            {
                var token = new Token(new char[Token.MaximumLength], 0);
                token.Set(input);
                token.Remove(startIndex);

                Assert.Equal(expectedString, token.ToString());
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }
    }
}
