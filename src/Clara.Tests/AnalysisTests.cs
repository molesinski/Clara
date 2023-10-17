using Clara.Analysis;
using Xunit;

namespace Clara.Tests
{
    public class AnalysisTests
    {
        [Fact]
        public void BasicTokenization()
        {
            var phrase = "Ben&Jerry's SETI@home o.b. __ __'s __1__2 __a__b 3/4 56-78 cd-ef 1.234 1,234 1.234,45 -1";

            var actual = string.Join(" | ", new BasicTokenizer().CreateTokenTermSource().GetTerms(phrase));
            var expected = string.Join(" | ", new LuceneStandardTokenizer().CreateTokenTermSource().GetTerms(phrase));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BasicAnalysis()
        {
            var phrase = "Ben&Jerry's SETI@home o.b. __ __'s __1__2 __a__b 3/4 56-78 cd-ef 1.234 1,234 1.234,45 -1";

            var actual = string.Join(" | ", new BasicAnalyzer().CreateTokenTermSource().GetTerms(phrase));
            var expected = string.Join(" | ", new LuceneStandardAnalyzer().CreateTokenTermSource().GetTerms(phrase));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BasicEnglishAnalysis()
        {
            var phrase = "Ben&Jerry's SETI@home o.b. __ __'s __1__2 __a__b 3/4 56-78 cd-ef 1.234 1,234 1.234,45 -1";

            var actual = string.Join(" | ", new PorterAnalyzer().CreateTokenTermSource().GetTerms(phrase));
            var expected = string.Join(" | ", new LuceneEnglishAnalyzer().CreateTokenTermSource().GetTerms(phrase));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AdvancedEnglishAnalysis()
        {
            var porterAnalyzer = new PorterAnalyzer();
            var englishAnalyzer = new EnglishAnalyzer(stopwords: new PorterStopTokenFilter().Stopwords);
            var luceneAnalyzer = new LuceneEnglishAnalyzer();

            var phrases =
                new[]
                {
                    "Here is a small fact: You are going to die.",
                    "All this happened, more or less.",
                    "No live organism can continue for long to exist sanely under conditions of absolute reality walked alone.",
                    "When I stepped out into the bright sunlight from the darkness of the movie house, I had only two things on my mind: Paul Newman and a ride home.",
                    "The snow in the mountains was melting and Bunny had been dead for several weeks before we came to understand the gravity of our situation.",
                    "It was a queer, sultry summer, the summer they electrocuted the Rosenbergs, and I didn't know what I was doing in New York.",
                    "It is a truth universally acknowledged, that a single man in possession of a good fortune must be in want of a wife.",
                    "My name was Salmon, like the fish; first name, Susie. I was fourteen when I was murdered on December 6, 1973.",
                    "This is my favorite book in all the world, though I have never read it.",
                    "Sometimes you need to scorch everything to the ground and start over. After the burning the soil is richer, and new things can grow. People are like that, too.",
                    "Life changes fast. Life changes in the instant. You sit down to dinner and life as you know it ends. The question of self-pity.",
                };

            foreach (var phrase in phrases)
            {
                var porterTokens = string.Join(" | ", porterAnalyzer.CreateTokenTermSource().GetTerms(phrase));
                var englishTokens = string.Join(" | ", englishAnalyzer.CreateTokenTermSource().GetTerms(phrase));
                var luceneTokens = string.Join(" | ", luceneAnalyzer.CreateTokenTermSource().GetTerms(phrase));

                Assert.NotEqual(string.Empty, porterTokens);
                Assert.Equal(porterTokens, englishTokens);
                Assert.Equal(porterTokens, luceneTokens);
            }
        }
    }
}
