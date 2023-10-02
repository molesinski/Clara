using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class SynonymGraphTests
    {
        private readonly SynonymGraph synonymMap;

        public SynonymGraphTests()
        {
            this.synonymMap =
                new SynonymGraph(
                    ProductMapper.Analyzer,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                        new ExplicitMappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
                        new ExplicitMappingSynonym(new[] { "i pod", "i-pod", "ipod" }, new[] { "i pod", "i-pod", "ipod" }),
                        new EquivalencySynonym(new[] { "aaa", "bbb" }),
                        new EquivalencySynonym(new[] { "bbb", "ccc" }),
                        new EquivalencySynonym(new[] { "ccc", "ddd" }),
                        new ExplicitMappingSynonym(new[] { "ddd" }, new[] { "eee" }),
                        new ExplicitMappingSynonym(new[] { "eee" }, new[] { "fff" }),
                        new ExplicitMappingSynonym(new[] { "fff" }, new[] { "ggg" }),
                    });
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
        //// Equivalency
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
        //// ExplicitMapping
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
        //// ExplicitMapping (TOTAL)
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
        //// Cross synonym
        [InlineData("aaa", SearchMode.All, "bbb", true)]
        [InlineData("aaa", SearchMode.Any, "bbb", true)]
        [InlineData("aaa", SearchMode.All, "ddd", true)]
        [InlineData("aaa", SearchMode.Any, "ddd", true)]
        [InlineData("aaa", SearchMode.All, "eee", true)]
        [InlineData("aaa", SearchMode.Any, "eee", true)]
        [InlineData("bbb", SearchMode.All, "fff", true)]
        [InlineData("bbb", SearchMode.Any, "fff", true)]
        [InlineData("ccc", SearchMode.All, "ggg", true)]
        [InlineData("ccc", SearchMode.Any, "ggg", true)]
        [InlineData("ddd", SearchMode.All, "ggg", true)]
        [InlineData("ddd", SearchMode.Any, "ggg", true)]
        public void IsMatching(string search, SearchMode mode, string document, bool expected)
        {
            var documentTokens = this.synonymMap.GetTokens(document).ToArray();

            var matchExpression =
                mode == SearchMode.All
                    ? Match.All(this.synonymMap.Analyzer.GetTokens(search))
                    : Match.Any(this.synonymMap.Analyzer.GetTokens(search));

            matchExpression = this.synonymMap.Process(matchExpression);

            var isMatching = matchExpression.IsMatching(documentTokens);

            Assert.Equal(expected, isMatching);
        }
    }
}
