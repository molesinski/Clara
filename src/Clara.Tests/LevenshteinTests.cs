#pragma warning disable CA5394 // Do not use insecure randomness

using Clara.Utils;
using Xunit;

namespace Clara.Tests
{
    public class LevenshteinTests
    {
        [Theory]
        [InlineData("", "", 0)]
        [InlineData("", "abc", 3)]
        [InlineData("abc", "abc", 0)]
        [InlineData("kitten", "sitting", 3)]
        [InlineData("flaw", "lawn", 2)]
        [InlineData("chocolate", "chocolote", 1)]
        [InlineData("abcdefgh", "hgfedcba", 8)]
        public void CalculatesDistance(string a, string b, int expected)
        {
            Assert.Equal(expected, Levenshtein.CalculateDistance(a.AsSpan(), b.AsSpan()));
            Assert.Equal(expected, Levenshtein.CalculateDistance(b.AsSpan(), a.AsSpan()));
            Assert.Equal(expected, Levenshtein.CalculateDistance(a.AsSpan(), b.AsSpan(), expected));
        }

        [Theory]
        [InlineData("kitten", "sitting", 2)]
        [InlineData("abc", "abcdefgh", 4)]
        [InlineData("abcdefghijklmnop", "ponmlkjihgfedcba", 1)]
        [InlineData("chocolate", "vanilla", 0)]
        public void ReturnsValueAboveMaximumWhenDistanceExceedsIt(string a, string b, int maximumDistance)
        {
            Assert.True(Levenshtein.CalculateDistance(a.AsSpan(), b.AsSpan(), maximumDistance) > maximumDistance);
        }

        [Fact]
        public void BoundedDistanceMatchesFullDistanceWithinMaximum()
        {
            var random = new Random(12345);

            for (var i = 0; i < 5_000; i++)
            {
                var a = CreateString(random);
                var b = random.Next(3) == 0 ? Mutate(a, random) : CreateString(random);
                var expected = CalculateReferenceDistance(a, b);

                for (var maximumDistance = 0; maximumDistance <= 8; maximumDistance++)
                {
                    var actual = Levenshtein.CalculateDistance(a.AsSpan(), b.AsSpan(), maximumDistance);

                    if (expected <= maximumDistance)
                    {
                        Assert.Equal(expected, actual);
                    }
                    else
                    {
                        Assert.True(actual > maximumDistance, $"'{a}' '{b}' maximum {maximumDistance}: {actual}");
                    }
                }

                Assert.Equal(expected, Levenshtein.CalculateDistance(a.AsSpan(), b.AsSpan()));
            }
        }

        [Fact]
        public void NegativeMaximumDistanceIsRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Levenshtein.CalculateDistance("a".AsSpan(), "b".AsSpan(), -1));
        }

        private static string CreateString(Random random)
        {
            var chars = new char[random.Next(0, 16)];

            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)('a' + random.Next(4));
            }

            return new string(chars);
        }

        private static string Mutate(string value, Random random)
        {
            var chars = value.ToList();
            var edits = random.Next(1, 4);

            for (var i = 0; i < edits; i++)
            {
                var position = random.Next(chars.Count + 1);

                switch (random.Next(3))
                {
                    case 0:
                        chars.Insert(position, (char)('a' + random.Next(4)));
                        break;
                    case 1 when position < chars.Count:
                        chars.RemoveAt(position);
                        break;
                    case 2 when position < chars.Count:
                        chars[position] = (char)('a' + random.Next(4));
                        break;
                }
            }

            return new string(chars.ToArray());
        }

        private static int CalculateReferenceDistance(string a, string b)
        {
            var previous = new int[b.Length + 1];
            var current = new int[b.Length + 1];

            for (var j = 0; j <= b.Length; j++)
            {
                previous[j] = j;
            }

            for (var i = 1; i <= a.Length; i++)
            {
                current[0] = i;

                for (var j = 1; j <= b.Length; j++)
                {
                    var cost = a[i - 1] == b[j - 1] ? 0 : 1;

                    current[j] = Math.Min(Math.Min(previous[j] + 1, current[j - 1] + 1), previous[j - 1] + cost);
                }

                (previous, current) = (current, previous);
            }

            return previous[b.Length];
        }
    }
}
