using Clara.Analysis;
using Xunit;

namespace Clara.Tests
{
    public class TokenTests
    {
        [Theory]
        [InlineData("", "", "")]
        [InlineData("abc", "", "abc")]
        [InlineData("", "abc", "abc")]
        [InlineData("abc", "def", "abcdef")]
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

                Assert.Equal(new Token(expectedString), token);
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, "", typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, "", "")]
        [InlineData("abc", 0, "", "")]
        [InlineData("abc", 1, "", "a")]
        [InlineData("abc", 2, "", "ab")]
        [InlineData("abc", 3, "", "abc")]
        [InlineData("", 0, "abc", "abc")]
        [InlineData("abc", 3, "def", "abcdef")]
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

                Assert.Equal(new Token(expectedString), token);
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, "", typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, "", "")]
        [InlineData("abc", 0, "", "abc")]
        [InlineData("abc", 1, "", "abc")]
        [InlineData("abc", 3, "", "abc")]
        [InlineData("", 0, "abc", "abc")]
        [InlineData("abc", 3, "def", "abcdef")]
        [InlineData("abc", 1, "def", "adefbc")]
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

                Assert.Equal(new Token(expectedString), token);
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

                Assert.Equal(new Token(expectedString), token);
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }

        [Theory]
        [InlineData("", 1, typeof(ArgumentOutOfRangeException))]
        [InlineData("", 0, "")]
        [InlineData("abc", 0, "")]
        [InlineData("abc", 1, "a")]
        [InlineData("abc", 2, "ab")]
        [InlineData("abc", 3, "abc")]
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

                Assert.Equal(new Token(expectedString), token);
            }
            else
            {
                throw new InvalidOperationException("Invalid inline data expected value type.");
            }
        }
    }
}
