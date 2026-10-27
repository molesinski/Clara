#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class TokenCorrectionTests
    {
        [Fact]
        public void ExactModeDoesNotCorrectMisspelledTokens()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolote"));

            Assert.Empty(result.Documents);
        }

        [Fact]
        public void CorrectModeMatchesMisspelledTokenWithBoostedScore()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolote", matchMode: MatchMode.Correct));

            Assert.Equal(new[] { "chocolate-bar", "chocolate-cookies" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            Assert.All(result.Documents, x => Assert.Equal(TokenMatching.Default.GetBoost(1, isPrefix: false), x.Score));
        }

        [Fact]
        public void CorrectModeUsesOnlyLowestCostCandidates()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cookias", matchMode: MatchMode.Correct));

            Assert.Equal("chocolate-cookies", Assert.Single(result.Documents).Key);
        }

        [Fact]
        public void CorrectPrefixModeUsesAllCandidatesForLastTerm()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cookias", matchMode: MatchMode.CorrectPrefix));

            Assert.Equal(new[] { "chocolate-cookies", "slow-cookers" }, result.Documents.Select(x => x.Key));
        }

        [Fact]
        public void CorrectModeKeepsTermsWithExactMatches()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cable", matchMode: MatchMode.Correct));

            Assert.Equal("cable-ties", Assert.Single(result.Documents).Key);
        }

        [Theory]
        [InlineData("choc", new[] { "chocolate-bar", "chocolate-cookies" })]
        [InlineData("choc ", new string[0])]
        [InlineData("chocolate bar", new[] { "chocolate-bar" })]
        public void CorrectPrefixModeCompletesOnlyUnfinishedLastTerm(string text, string[] expected)
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, text, matchMode: MatchMode.CorrectPrefix));

            Assert.Equal(expected, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
        }

        [Fact]
        public void CorrectPrefixModeRanksExactTokenAboveCompletion()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red", matchMode: MatchMode.CorrectPrefix));

            Assert.Collection(
                result.Documents,
                x =>
                {
                    Assert.Equal("red-apple", x.Key);
                    Assert.Equal(1f, x.Score);
                },
                x =>
                {
                    Assert.Equal("redwood-table", x.Key);
                    Assert.Equal(TokenMatching.Default.GetBoost(0, isPrefix: true), x.Score);
                });
        }

        [Fact]
        public void CorrectModePrefersVariantOverFuzzyMatch()
        {
            var mapper = new CorrectionMapper(new[] { new KeyValuePair<string, string>("colour", "color") });
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "colour", matchMode: MatchMode.Correct));

            var document = Assert.Single(result.Documents);

            Assert.Equal("color-pencils", document.Key);
            Assert.Equal(1f, document.Score);
        }

        [Fact]
        public void CorrectModeCombinesCorrectedAndExactTerms()
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolote cookies", matchMode: MatchMode.Correct));

            var document = Assert.Single(result.Documents);

            Assert.Equal("chocolate-cookies", document.Key);
            Assert.Equal(1f + TokenMatching.Default.GetBoost(1, isPrefix: false), document.Score);
        }

        [Fact]
        public void TokenCorrectionRequiresFieldTokenizer()
        {
            var correction = new TokenCorrection(new Analyzer(new WhitespaceTokenizer(), new LowerInvariantTokenFilter()));

            Assert.Throws<ArgumentException>(() => new TextField<CorrectionDocument>(x => x.Text, new PorterAnalyzer(), tokenCorrection: correction));
        }

        [Fact]
        public void CorrectingModesRequireTokenCorrection()
        {
            var field = new TextField<CorrectionDocument>(x => x.Text, new PorterAnalyzer());

            Assert.Throws<ArgumentException>(() => new TextSearchExpression(field, SearchMode.All, "red", matchMode: MatchMode.Correct));
        }
    }
}
