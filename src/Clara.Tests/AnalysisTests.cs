using Clara.Analysis;
using Xunit;

namespace Clara.Tests
{
    public class AnalysisTests
    {
        [Fact]
        public void EnglishAnalysis()
        {
            var porterAnalyzer =
                new Analyzer(
                    new BasicTokenizer(),
                    new LowerInvariantTokenFilter(),
                    new PorterPossessiveTokenFilter(),
                    new PorterStopTokenFilter(),
                    new PorterStemTokenFilter());

            var englishAnalyzer =
                new Analyzer(
                    new BasicTokenizer(),
                    new LowerInvariantTokenFilter(),
                    new PorterStopTokenFilter(),
                    new EnglishStemTokenFilter());

            var luceneEnglishAnalyzer =
                new LuceneEnglishAnalyzer();

            var lucenePorterAnalyzer =
                new Analyzer(
                    new LuceneStandardTokenizer(),
                    new LowerInvariantTokenFilter(),
                    new PorterPossessiveTokenFilter(),
                    new LucenePorterStopTokenFilter(),
                    new LucenePorterStemTokenFilter());

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
                var porterTokens = porterAnalyzer.GetTerms(phrase).Select(x => x.Token.ToReadOnly()).ToArray();
                var englishTokens = englishAnalyzer.GetTerms(phrase).Select(x => x.Token.ToReadOnly()).ToArray();
                var luceneEnglishTokens = luceneEnglishAnalyzer.GetTerms(phrase).Select(x => x.Token.ToReadOnly()).ToArray();
                var lucenePorterTokens = lucenePorterAnalyzer.GetTerms(phrase).Select(x => x.Token.ToReadOnly()).ToArray();

                Assert.True(porterTokens.Length > 0);
                Assert.Equal(porterTokens.Length, englishTokens.Length);
                Assert.Equal(porterTokens.Length, luceneEnglishTokens.Length);
                Assert.Equal(porterTokens.Length, lucenePorterTokens.Length);

                var length = porterTokens.Length;

                for (var i = 0; i < length; i++)
                {
                    Assert.Equal(porterTokens[i], englishTokens[i]);
                    Assert.Equal(porterTokens[i], luceneEnglishTokens[i]);
                    Assert.Equal(porterTokens[i], lucenePorterTokens[i]);
                }
            }
        }
    }
}
