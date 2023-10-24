using Clara.Analysis;
using Clara.Analysis.MatchExpressions;
using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;
using Xunit.Abstractions;

namespace Clara.Tests
{
    public class SynonymMapTests
    {
        private readonly ITestOutputHelper output;

        public SynonymMapTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        //// Baseline
        [InlineData("", SearchMode.All, "", false)]
        [InlineData("", SearchMode.Any, "", false)]
        [InlineData("xxx", SearchMode.All, "", false)]
        [InlineData("xxx", SearchMode.Any, "", false)]
        [InlineData("", SearchMode.All, "xxx", false)]
        [InlineData("", SearchMode.Any, "xxx", false)]
        [InlineData("xxx", SearchMode.All, "xxx", true)]
        [InlineData("xxx", SearchMode.Any, "xxx", true)]
        //// Equivalency : i pad, i-pad, ipad
        [InlineData("i-pad", SearchMode.All, "", false)]
        [InlineData("i-pad", SearchMode.Any, "", false)]
        [InlineData("i-pad", SearchMode.All, "xxx", false)]
        [InlineData("i-pad", SearchMode.Any, "xxx", false)]
        [InlineData("i pad", SearchMode.All, "i pad", true)]
        [InlineData("i pad", SearchMode.Any, "i pad", true)]
        [InlineData("i-pad", SearchMode.All, "i pad", true)]
        [InlineData("i-pad", SearchMode.Any, "i-pad", true)]
        [InlineData("ipad", SearchMode.All, "ipad", true)]
        [InlineData("ipad", SearchMode.Any, "ipad", true)]
        [InlineData("i-pad", SearchMode.All, "ipad", true)]
        [InlineData("i-pad", SearchMode.Any, "ipad", true)]
        [InlineData("ipad", SearchMode.All, "i pad", true)]
        [InlineData("ipad", SearchMode.Any, "i pad", true)]
        [InlineData("i pad", SearchMode.All, "i-pad", true)]
        [InlineData("i pad", SearchMode.Any, "i-pad", true)]
        [InlineData("xxx i pad", SearchMode.All, "ipad", false)]
        [InlineData("xxx i pad", SearchMode.Any, "ipad", true)]
        [InlineData("xxx i-pad", SearchMode.All, "ipad", false)]
        [InlineData("xxx i-pad", SearchMode.Any, "ipad", true)]
        [InlineData("xxx ipad", SearchMode.All, "ipad", false)]
        [InlineData("xxx ipad", SearchMode.Any, "ipad", true)]
        [InlineData("pad", SearchMode.All, "i pad", false)]
        [InlineData("pad", SearchMode.Any, "i pad", false)]
        [InlineData("pad", SearchMode.All, "ipad", false)]
        [InlineData("pad", SearchMode.Any, "ipad", false)]
        //// ExplicitMapping : i phone, i-phone => iphone
        [InlineData("i-phone", SearchMode.All, "", false)]
        [InlineData("i-phone", SearchMode.Any, "", false)]
        [InlineData("i-phone", SearchMode.All, "xxx", false)]
        [InlineData("i-phone", SearchMode.Any, "xxx", false)]
        [InlineData("i phone", SearchMode.All, "i phone", false /* true */)]
        [InlineData("i phone", SearchMode.Any, "i phone", false /* true */)]
        [InlineData("i-phone", SearchMode.All, "i phone", false /* true */)]
        [InlineData("i-phone", SearchMode.Any, "i-phone", false /* true */)]
        [InlineData("iphone", SearchMode.All, "iphone", true)]
        [InlineData("iphone", SearchMode.Any, "iphone", true)]
        [InlineData("i-phone", SearchMode.All, "iphone", false /* true */)]
        [InlineData("i-phone", SearchMode.Any, "iphone", false /* true */)]
        [InlineData("iphone", SearchMode.All, "i phone", true)]
        [InlineData("iphone", SearchMode.Any, "i phone", true)]
        [InlineData("i phone", SearchMode.All, "i-phone", false /* true */)]
        [InlineData("i phone", SearchMode.Any, "i-phone", false /* true */)]
        [InlineData("xxx i phone", SearchMode.All, "iphone", false)]
        [InlineData("xxx i phone", SearchMode.Any, "iphone", false /* true */)]
        [InlineData("xxx i-phone", SearchMode.All, "iphone", false)]
        [InlineData("xxx i-phone", SearchMode.Any, "iphone", false /* true */)]
        [InlineData("xxx iphone", SearchMode.All, "iphone", false)]
        [InlineData("xxx iphone", SearchMode.Any, "iphone", true)]
        [InlineData("phone", SearchMode.All, "i phone", false)]
        [InlineData("phone", SearchMode.Any, "i phone", false)]
        [InlineData("phone", SearchMode.All, "iphone", false)]
        [InlineData("phone", SearchMode.Any, "iphone", false)]
        //// ExplicitMapping : i pod, i-pod, ipod => i pod, i-pod, ipod
        [InlineData("i-pod", SearchMode.All, "", false)]
        [InlineData("i-pod", SearchMode.Any, "", false)]
        [InlineData("i-pod", SearchMode.All, "xxx", false)]
        [InlineData("i-pod", SearchMode.Any, "xxx", false)]
        [InlineData("i pod", SearchMode.All, "i pod", true)]
        [InlineData("i pod", SearchMode.Any, "i pod", true)]
        [InlineData("i-pod", SearchMode.All, "i pod", true)]
        [InlineData("i-pod", SearchMode.Any, "i-pod", true)]
        [InlineData("ipod", SearchMode.All, "ipod", true)]
        [InlineData("ipod", SearchMode.Any, "ipod", true)]
        [InlineData("i-pod", SearchMode.All, "ipod", true)]
        [InlineData("i-pod", SearchMode.Any, "ipod", true)]
        [InlineData("ipod", SearchMode.All, "i pod", true)]
        [InlineData("ipod", SearchMode.Any, "i pod", true)]
        [InlineData("i pod", SearchMode.All, "i-pod", true)]
        [InlineData("i pod", SearchMode.Any, "i-pod", true)]
        [InlineData("xxx i pod", SearchMode.All, "ipod", false)]
        [InlineData("xxx i pod", SearchMode.Any, "ipod", true)]
        [InlineData("xxx i-pod", SearchMode.All, "ipod", false)]
        [InlineData("xxx i-pod", SearchMode.Any, "ipod", true)]
        [InlineData("xxx ipod", SearchMode.All, "ipod", false)]
        [InlineData("xxx ipod", SearchMode.Any, "ipod", true)]
        [InlineData("pod", SearchMode.All, "i pod", false)]
        [InlineData("pod", SearchMode.Any, "i pod", false)]
        [InlineData("pod", SearchMode.All, "ipod", false)]
        [InlineData("pod", SearchMode.Any, "ipod", false)]
        //// ExplicitMapping : imac => i mac, i-mac, imac
        [InlineData("i-mac", SearchMode.All, "", false)]
        [InlineData("i-mac", SearchMode.Any, "", false)]
        [InlineData("i-mac", SearchMode.All, "xxx", false)]
        [InlineData("i-mac", SearchMode.Any, "xxx", false)]
        [InlineData("i mac", SearchMode.All, "i mac", true)]
        [InlineData("i mac", SearchMode.Any, "i mac", true)]
        [InlineData("i-mac", SearchMode.All, "i mac", true)]
        [InlineData("i-mac", SearchMode.Any, "i-mac", true)]
        [InlineData("imac", SearchMode.All, "imac", true)]
        [InlineData("imac", SearchMode.Any, "imac", true)]
        [InlineData("i-mac", SearchMode.All, "imac", true)]
        [InlineData("i-mac", SearchMode.Any, "imac", true)]
        [InlineData("imac", SearchMode.All, "i mac", false /* true */)]
        [InlineData("imac", SearchMode.Any, "i mac", false /* true */)]
        [InlineData("i mac", SearchMode.All, "i-mac", true)]
        [InlineData("i mac", SearchMode.Any, "i-mac", true)]
        [InlineData("xxx i mac", SearchMode.All, "imac", false)]
        [InlineData("xxx i mac", SearchMode.Any, "imac", true)]
        [InlineData("xxx i-mac", SearchMode.All, "imac", false)]
        [InlineData("xxx i-mac", SearchMode.Any, "imac", true)]
        [InlineData("xxx imac", SearchMode.All, "imac", false)]
        [InlineData("xxx imac", SearchMode.Any, "imac", true)]
        [InlineData("mac", SearchMode.All, "i mac", true /* false */)]
        [InlineData("mac", SearchMode.Any, "i mac", true /* false */)]
        [InlineData("mac", SearchMode.All, "imac", false)]
        [InlineData("mac", SearchMode.Any, "imac", false)]
        public void StandardMatches(string search, SearchMode mode, string document, bool expected)
        {
            var synonymMap =
                new SynonymMap(
                    new StandardAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                        new MappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
                        new MappingSynonym(new[] { "i pod", "i-pod", "ipod" }, new[] { "i pod", "i-pod", "ipod" }),
                        new MappingSynonym(new[] { "imac" }, new[] { "i mac", "i-mac", "imac" }),
                    });

            Assert.Equal(expected, IsMatching(synonymMap, search, mode, document, this.output));
        }

        [Fact]
        public void CrossMatches()
        {
            var synonymMap =
                new SynonymMap(
                    new StandardAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "aaa", "bbb" }),
                        new EquivalencySynonym(new[] { "bbb", "ccc" }),
                        new MappingSynonym(new[] { "ccc" }, new[] { "ddd" }),
                        new EquivalencySynonym(new[] { "ddd", "eee" }),
                        new MappingSynonym(new[] { "eee" }, new[] { "fff" }),
                        new MappingSynonym(new[] { "fff" }, new[] { "ggg" }),
                    });

            var allPhrases = Array.Empty<string>()
                .Concat(synonymMap.Synonyms.SelectMany(x => x.Phrases))
                .Concat(synonymMap.Synonyms.OfType<MappingSynonym>().SelectMany(x => x.MappedPhrases))
                .Distinct()
                .ToList();

            var finalPhrase = Array.Empty<string>()
                .Concat(synonymMap.Synonyms.OfType<MappingSynonym>().SelectMany(x => x.MappedPhrases))
                .Last();

            foreach (var phrase in allPhrases)
            {
                Assert.False(IsMatching(synonymMap, phrase, SearchMode.All, string.Empty, output: null));
                Assert.False(IsMatching(synonymMap, phrase, SearchMode.Any, string.Empty, output: null));
                Assert.False(IsMatching(synonymMap, string.Empty, SearchMode.All, phrase, output: null));
                Assert.False(IsMatching(synonymMap, string.Empty, SearchMode.Any, phrase, output: null));
                Assert.False(IsMatching(synonymMap, phrase, SearchMode.All, "xxx", output: null));
                Assert.False(IsMatching(synonymMap, phrase, SearchMode.Any, "xxx", output: null));
                Assert.False(IsMatching(synonymMap, "xxx", SearchMode.All, phrase, output: null));
                Assert.False(IsMatching(synonymMap, "xxx", SearchMode.Any, phrase, output: null));

                Assert.True(IsMatching(synonymMap, finalPhrase, SearchMode.All, phrase, output: null));
                Assert.True(IsMatching(synonymMap, finalPhrase, SearchMode.Any, phrase, output: null));
            }
        }

        [Fact]
        public void BacktrackingPosition()
        {
            var synonymMap =
                new SynonymMap(
                    new StandardAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "bbb", "ccc" }),
                        new EquivalencySynonym(new[] { "mmm nnn ooo", "ppp" }),
                    });

            var phrase = "aaa a a bbb a a mmm nnn a a mmm nnn";

            var input = synonymMap.Analyzer.CreateTokenTermSource().GetTerms(phrase).Select(x => new SearchTerm(x.Token.ToString(), x.Position)).ToList();
            var output = input.ToList();

            synonymMap.Process(SearchMode.All, output);

            var expected = string.Join(", ", input.Select(x => x.Position).Distinct().OrderBy(x => x));
            var actual = string.Join(", ", output.Select(x => x.Position).Distinct().OrderBy(x => x));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TextParsing()
        {
            var synonyms =
                new Synonym[]
                {
                    new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                    new MappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
                    new MappingSynonym(new[] { "i pod", "i-pod", "ipod" }, new[] { "i pod", "i-pod", "ipod" }),
                    new MappingSynonym(new[] { "imac" }, new[] { "i mac", "i-mac", "imac" }),
                };

            var synonymsText = string.Join(Environment.NewLine, synonyms.Select(x => x.ToString()));

            Assert.Equal(
                """
                i pad, i-pad, ipad
                i phone, i-phone => iphone
                i pod, i-pod, ipod => i pod, i-pod, ipod
                imac => i mac, i-mac, imac
                """,
                synonymsText);

            var parsed = Synonym.Parse(synonymsText);

            Assert.True(synonyms.Select(x => x.ToString()).SequenceEqual(parsed.Select(x => x.ToString())));
        }

        private static bool IsMatching(ISynonymMap synonymMap, string search, SearchMode mode, string document, ITestOutputHelper? output)
        {
            var documentTokens = synonymMap.CreateTokenTermSource().GetTerms(document).Select(x => x.Token.ToString()).ToList();
            var searchTerms = synonymMap.Analyzer.CreateTokenTermSource().GetTerms(search).Select(x => new SearchTerm(x.Token.ToString(), x.Position)).ToList();

            synonymMap.Process(mode, searchTerms);

            var matchExpression = Match.Search(mode, searchTerms);

            var isMatching = matchExpression.Matches(documentTokens);

            if (output is not null)
            {
                output.WriteLine(string.Concat("Document: ", string.Join(", ", documentTokens.Select(x => $"\"{x}\""))));
                output.WriteLine(string.Concat("Expression: ", matchExpression.ToString()));
            }

            return isMatching;
        }
    }
}
