using Clara.Analysis;
using Xunit;

namespace Clara.Tests
{
    public class MorfologikTests
    {
        [Theory]
        [InlineData("mielone", "mielony")]
        [InlineData("mielony", "mielony")]
        [InlineData("mielona", "mielony")]
        [InlineData("mielonych", "mielony")]
        [InlineData("czerwone", "czerwony")]
        [InlineData("czerwony", "czerwony")]
        [InlineData("mała", "mały")]
        [InlineData("małe", "mały")]
        [InlineData("mały", "mały")]
        [InlineData("białe", "biały")]
        [InlineData("biały", "biały")]
        [InlineData("lodów", "lody")]
        [InlineData("lody", "lody")]
        [InlineData("dane", "dany")]
        [InlineData("danych", "dany")]
        [InlineData("suszone", "suszyć")]
        [InlineData("pieczone", "piec")]
        [InlineData("tanie", "tani")]
        [InlineData("rady", "rad")]
        [InlineData("", "")]
        [InlineData("qzxqzxqzx", "qzxqzxqzx")]
        public void SelectsExpectedLemma(string word, string expected)
        {
            var filter = new MorfologikStemTokenFilter();
            var token = new Token(new char[Token.MaximumLength], 0);
            token.Set(word);

            var actual = filter.Process(token, UnexpectedNext);

            Assert.Equal(new Token(expected), actual);
        }

        [Theory]
        [InlineData("MIELONE mielony mielona mielonych", "mielony", 4)]
        [InlineData("CZERWONE czerwony", "czerwony", 2)]
        [InlineData("MAŁA małe mały", "mały", 3)]
        [InlineData("BIAŁE biały", "biały", 2)]
        [InlineData("LODÓW lody", "lody", 2)]
        public void AnalyzerConflatesRelatedForms(string phrase, string expected, int expectedCount)
        {
            var source = new MorfologikAnalyzer(stopwords: Array.Empty<string>()).CreateTokenTermSource();
            var terms = source.GetTerms(phrase).Select(term => term.Token.ToString()).ToArray();

            Assert.Equal(expectedCount, terms.Length);
            Assert.All(terms, term => Assert.Equal(expected, term));
        }

        [Fact]
        public void AnalyzerUsesTheSameHeuristicInNounContext()
        {
            var source = new MorfologikAnalyzer(stopwords: Array.Empty<string>()).CreateTokenTermSource();
            var terms = source.GetTerms("dane osobowe").Select(term => term.Token.ToString()).ToArray();

            Assert.Equal(2, terms.Length);
            Assert.Equal("dany", terms[0]);
            Assert.Equal("osobowy", terms[1]);
        }

        [Fact]
        public void ReusedBuffersDoNotRetainThePreviousLemma()
        {
            var filter = new MorfologikStemTokenFilter();
            var token = new Token(new char[Token.MaximumLength], 0);

            for (var iteration = 0; iteration < 3; iteration++)
            {
                token.Set("mielonych");
                Assert.Equal(new Token("mielony"), filter.Process(token, UnexpectedNext));

                token.Set("suszone");
                Assert.Equal(new Token("suszyć"), filter.Process(token, UnexpectedNext));

                token.Set("qzxqzxqzx");
                Assert.Equal(new Token("qzxqzxqzx"), filter.Process(token, UnexpectedNext));

                token.Set(string.Empty);
                Assert.True(filter.Process(token, UnexpectedNext).IsEmpty);
            }
        }

        private static Token UnexpectedNext(Token token)
        {
            throw new InvalidOperationException("Morfologik stemming terminates the filter chain.");
        }
    }
}
