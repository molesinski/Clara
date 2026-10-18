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
        public void Append(string input, string chars, string expected)
        {
            var token = CreateToken(input);
            token.Append(chars);

            Assert.Equal(expected, token.ToString());
        }

        [Theory]
        [InlineData("", 0, "", "")]
        [InlineData("abc", 0, "", "")]
        [InlineData("abc", 1, "", "a")]
        [InlineData("abc", 2, "", "ab")]
        [InlineData("abc", 3, "", "abc")]
        [InlineData("", 0, "abc", "abc")]
        [InlineData("abc", 3, "def", "abcdef")]
        public void Write(string input, int startIndex, string chars, string expected)
        {
            var token = CreateToken(input);
            token.Write(startIndex, chars);

            Assert.Equal(expected, token.ToString());
        }

        [Fact]
        public void WriteRejectsStartBeyondLength()
        {
            var token = CreateToken(string.Empty);

            Assert.Throws<ArgumentOutOfRangeException>(() => token.Write(1, string.Empty));
        }

        [Theory]
        [InlineData("", 0, "", "")]
        [InlineData("abc", 0, "", "abc")]
        [InlineData("abc", 1, "", "abc")]
        [InlineData("abc", 3, "", "abc")]
        [InlineData("", 0, "abc", "abc")]
        [InlineData("abc", 3, "def", "abcdef")]
        [InlineData("abc", 1, "def", "adefbc")]
        public void Insert(string input, int startIndex, string chars, string expected)
        {
            var token = CreateToken(input);
            token.Insert(startIndex, chars);

            Assert.Equal(expected, token.ToString());
        }

        [Fact]
        public void InsertRejectsStartBeyondLength()
        {
            var token = CreateToken(string.Empty);

            Assert.Throws<ArgumentOutOfRangeException>(() => token.Insert(1, string.Empty));
        }

        [Theory]
        [InlineData("", 0, 0, "")]
        [InlineData("abc", 0, 3, "")]
        [InlineData("abc", 0, 1, "bc")]
        [InlineData("abc", 1, 2, "a")]
        [InlineData("abc", 1, 1, "ac")]
        [InlineData("abcde", 1, 3, "ae")]
        [InlineData("abc", 1, 0, "abc")]
        [InlineData("abc", 2, 1, "ab")]
        [InlineData("abc", 3, 0, "abc")]
        public void Delete(string input, int startIndex, int count, string expected)
        {
            var token = CreateToken(input);
            token.Delete(startIndex, count);

            Assert.Equal(expected, token.ToString());
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        public void DeleteRejectsRangeBeyondLength(int startIndex, int count)
        {
            var token = CreateToken(string.Empty);

            Assert.Throws<ArgumentOutOfRangeException>(() => token.Delete(startIndex, count));
        }

        [Theory]
        [InlineData("", 0, "")]
        [InlineData("abc", 0, "")]
        [InlineData("abc", 1, "a")]
        [InlineData("abc", 2, "ab")]
        [InlineData("abc", 3, "abc")]
        public void Remove(string input, int startIndex, string expected)
        {
            var token = CreateToken(input);
            token.Remove(startIndex);

            Assert.Equal(expected, token.ToString());
        }

        [Fact]
        public void RemoveRejectsStartBeyondLength()
        {
            var token = CreateToken(string.Empty);

            Assert.Throws<ArgumentOutOfRangeException>(() => token.Remove(1));
        }

        private static Token CreateToken(string input)
        {
            var token = new Token(new char[Token.MaximumLength], 0);
            token.Set(input);
            return token;
        }
    }
}
