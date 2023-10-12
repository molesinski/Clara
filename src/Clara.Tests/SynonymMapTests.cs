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
        private readonly IAnalyzer analyzer;
        private SynonymMap? standardSynonymMap;

        public SynonymMapTests(ITestOutputHelper output)
        {
            this.output = output;
            this.analyzer = new BasicAnalyzer();
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
        [InlineData("i pad", SearchMode.All, "i pad", true)] // identity
        [InlineData("i pad", SearchMode.Any, "i pad", true)] // identity
        [InlineData("i-pad", SearchMode.All, "i pad", true)] // identity
        [InlineData("i-pad", SearchMode.Any, "i-pad", true)] // identity
        [InlineData("ipad", SearchMode.All, "ipad", true)] // identity
        [InlineData("ipad", SearchMode.Any, "ipad", true)] // identity
        [InlineData("i-pad", SearchMode.All, "ipad", true)] // transitive
        [InlineData("i-pad", SearchMode.Any, "ipad", true)] // transitive
        [InlineData("ipad", SearchMode.All, "i pad", true)] // transitive
        [InlineData("ipad", SearchMode.Any, "i pad", true)] // transitive
        [InlineData("i pad", SearchMode.All, "i-pad", true)] // transitive
        [InlineData("i pad", SearchMode.Any, "i-pad", true)] // transitive
        [InlineData("xxx i pad", SearchMode.All, "ipad", false)]
        [InlineData("xxx i pad", SearchMode.Any, "ipad", true)]
        [InlineData("xxx i-pad", SearchMode.All, "ipad", false)]
        [InlineData("xxx i-pad", SearchMode.Any, "ipad", true)]
        [InlineData("xxx ipad", SearchMode.All, "ipad", false)]
        [InlineData("xxx ipad", SearchMode.Any, "ipad", true)]
        [InlineData("pad", SearchMode.All, "i pad", true)]
        [InlineData("pad", SearchMode.Any, "i pad", true)]
        [InlineData("pad", SearchMode.All, "ipad", false)]
        [InlineData("pad", SearchMode.Any, "ipad", false)]
        //// ExplicitMapping : i phone, i-phone => iphone
        [InlineData("i-phone", SearchMode.All, "", false)]
        [InlineData("i-phone", SearchMode.Any, "", false)]
        [InlineData("i-phone", SearchMode.All, "xxx", false)]
        [InlineData("i-phone", SearchMode.Any, "xxx", false)]
        [InlineData("i phone", SearchMode.All, "i phone", true)] // identity
        [InlineData("i phone", SearchMode.Any, "i phone", true)] // identity
        [InlineData("i-phone", SearchMode.All, "i phone", true)] // identity
        [InlineData("i-phone", SearchMode.Any, "i-phone", true)] // identity
        [InlineData("iphone", SearchMode.All, "iphone", true)] // identity
        [InlineData("iphone", SearchMode.Any, "iphone", true)] // identity
        [InlineData("i-phone", SearchMode.All, "iphone", true)] // transitive
        [InlineData("i-phone", SearchMode.Any, "iphone", true)] // transitive
        [InlineData("iphone", SearchMode.All, "i phone", true)] // transitive
        [InlineData("iphone", SearchMode.Any, "i phone", true)] // transitive
        [InlineData("i phone", SearchMode.All, "i-phone", true)] // transitive
        [InlineData("i phone", SearchMode.Any, "i-phone", true)] // transitive
        [InlineData("xxx i phone", SearchMode.All, "iphone", false)]
        [InlineData("xxx i phone", SearchMode.Any, "iphone", true)]
        [InlineData("xxx i-phone", SearchMode.All, "iphone", false)]
        [InlineData("xxx i-phone", SearchMode.Any, "iphone", true)]
        [InlineData("xxx iphone", SearchMode.All, "iphone", false)]
        [InlineData("xxx iphone", SearchMode.Any, "iphone", true)]
        [InlineData("phone", SearchMode.All, "i phone", false /* true for Equivalency */)]
        [InlineData("phone", SearchMode.Any, "i phone", false /* true for Equivalency */)]
        [InlineData("phone", SearchMode.All, "iphone", false)]
        [InlineData("phone", SearchMode.Any, "iphone", false)]
        //// ExplicitMapping : i pod, i-pod, ipod => i pod, i-pod, ipod
        [InlineData("i-pod", SearchMode.All, "", false)]
        [InlineData("i-pod", SearchMode.Any, "", false)]
        [InlineData("i-pod", SearchMode.All, "xxx", false)]
        [InlineData("i-pod", SearchMode.Any, "xxx", false)]
        [InlineData("i pod", SearchMode.All, "i pod", true)] // identity
        [InlineData("i pod", SearchMode.Any, "i pod", true)] // identity
        [InlineData("i-pod", SearchMode.All, "i pod", true)] // identity
        [InlineData("i-pod", SearchMode.Any, "i-pod", true)] // identity
        [InlineData("ipod", SearchMode.All, "ipod", true)] // identity
        [InlineData("ipod", SearchMode.Any, "ipod", true)] // identity
        [InlineData("i-pod", SearchMode.All, "ipod", true)] // transitive
        [InlineData("i-pod", SearchMode.Any, "ipod", true)] // transitive
        [InlineData("ipod", SearchMode.All, "i pod", true)] // transitive
        [InlineData("ipod", SearchMode.Any, "i pod", true)] // transitive
        [InlineData("i pod", SearchMode.All, "i-pod", true)] // transitive
        [InlineData("i pod", SearchMode.Any, "i-pod", true)] // transitive
        [InlineData("xxx i pod", SearchMode.All, "ipod", false)]
        [InlineData("xxx i pod", SearchMode.Any, "ipod", true)]
        [InlineData("xxx i-pod", SearchMode.All, "ipod", false)]
        [InlineData("xxx i-pod", SearchMode.Any, "ipod", true)]
        [InlineData("xxx ipod", SearchMode.All, "ipod", false)]
        [InlineData("xxx ipod", SearchMode.Any, "ipod", true)]
        [InlineData("pod", SearchMode.All, "i pod", true)]
        [InlineData("pod", SearchMode.Any, "i pod", true)]
        [InlineData("pod", SearchMode.All, "ipod", true /* false for Equivalency */)]
        [InlineData("pod", SearchMode.Any, "ipod", true /* false for Equivalency */)]
        public void StandardMatches(string search, SearchMode mode, string document, bool expected)
        {
            this.standardSynonymMap ??=
                new SynonymMap(
                    this.analyzer,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                        new ExplicitMappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
                        new ExplicitMappingSynonym(new[] { "i pod", "i-pod", "ipod" }, new[] { "i pod", "i-pod", "ipod" }),
                    });

            Assert.Equal(expected, IsMatching(this.standardSynonymMap, search, mode, document, this.output));
        }

        [Fact]
        public void CrossMatches()
        {
            var synonymMap =
                new SynonymMap(
                    this.analyzer,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "aaa", "bbb" }),
                        new EquivalencySynonym(new[] { "bbb", "ccc" }),
                        new ExplicitMappingSynonym(new[] { "ccc" }, new[] { "ddd" }),
                        new EquivalencySynonym(new[] { "ddd", "eee" }),
                        new ExplicitMappingSynonym(new[] { "eee" }, new[] { "fff" }),
                        new ExplicitMappingSynonym(new[] { "fff" }, new[] { "ggg" }),
                    });

            var phrases = Array.Empty<string>()
                .Concat(synonymMap.Synonyms.SelectMany(x => x.Phrases))
                .Concat(synonymMap.Synonyms.OfType<ExplicitMappingSynonym>().SelectMany(x => x.MappedPhrases))
                .Distinct()
                .ToList();

            foreach (var a in phrases)
            {
                Assert.False(IsMatching(synonymMap, a, SearchMode.All, string.Empty, output: null));
                Assert.False(IsMatching(synonymMap, a, SearchMode.Any, string.Empty, output: null));
                Assert.False(IsMatching(synonymMap, string.Empty, SearchMode.All, a, output: null));
                Assert.False(IsMatching(synonymMap, string.Empty, SearchMode.Any, a, output: null));
                Assert.False(IsMatching(synonymMap, a, SearchMode.All, "xxx", output: null));
                Assert.False(IsMatching(synonymMap, a, SearchMode.Any, "xxx", output: null));
                Assert.False(IsMatching(synonymMap, "xxx", SearchMode.All, a, output: null));
                Assert.False(IsMatching(synonymMap, "xxx", SearchMode.Any, a, output: null));

                foreach (var b in phrases)
                {
                    Assert.True(IsMatching(synonymMap, a, SearchMode.All, b, output: null));
                    Assert.True(IsMatching(synonymMap, a, SearchMode.Any, b, output: null));
                }
            }
        }

        [Theory]
        [InlineData("aaa", 1, false)]
        [InlineData("bbb", 1, false)]
        [InlineData("xxx", 1, true)]
        [InlineData("aaa bbb", 2, true)]
        [InlineData("bbb aaa", 2, true)]
        [InlineData("xxx", 2, true)]
        public void PermutationMatches(string search, int permutatedTokenCountThreshold, bool expected)
        {
            var synonymMap =
                new SynonymMap(
                    this.analyzer,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "aaa bbb", "xxx" }),
                    },
                    permutatedTokenCountThreshold);

            Assert.Equal(expected, IsMatching(synonymMap, search, SearchMode.All, "xxx", output: null));
            Assert.Equal(expected, IsMatching(synonymMap, search, SearchMode.Any, "xxx", output: null));
        }

        [Fact]
        public void BacktrackingOrdinal()
        {
            var synonymMap =
                new SynonymMap(
                    this.analyzer,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "bbb", "ccc" }),
                        new EquivalencySynonym(new[] { "mmm nnn ooo", "ppp" }),
                    });

            var phrase = "aaa a a bbb a a mmm nnn a a mmm nnn";

            var input = synonymMap.Analyzer.GetTerms(phrase).Select(x => new SearchTerm(x.Ordinal, x.Token.ToString())).ToList();
            var output = input.ToList();

            synonymMap.Process(SearchMode.All, output);

            var expected = string.Join(", ", input.Select(x => x.Ordinal).Distinct().OrderBy(x => x));
            var actual = string.Join(", ", output.Select(x => x.Ordinal).Distinct().OrderBy(x => x));

            Assert.Equal(expected, actual);
        }

        private static bool IsMatching(ISynonymMap synonymMap, string search, SearchMode mode, string document, ITestOutputHelper? output)
        {
            var documentTokens = synonymMap.GetTerms(document).Select(x => x.Token.ToString()).ToList();
            var searchTerms = synonymMap.Analyzer.GetTerms(search).Select(x => new SearchTerm(x.Ordinal, x.Token.ToString())).ToList();

            synonymMap.Process(mode, searchTerms);

            var matchExpression = Match.Search(mode, searchTerms);

            var isMatching = matchExpression.IsMatching(documentTokens);

            if (output is not null)
            {
                output.WriteLine(string.Concat("Document: ", string.Join(", ", documentTokens.Select(x => $"\"{x}\""))));
                output.WriteLine(string.Concat("Expression: ", matchExpression.ToString()));
            }

            return isMatching;
        }
    }
}
