using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class SynonymMapTests
    {
        private readonly SynonymMap synonymMap;

        public SynonymMapTests()
        {
            this.synonymMap =
                new SynonymMap(
                    ProductMapper.Analyzer,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                        new ExplicitMappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
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
