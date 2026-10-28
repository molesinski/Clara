#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class TokenExpansionTests
    {
        [Fact]
        public void ExactSearchDoesNotMatchMisspelledTokens()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolote"));

            Assert.Empty(result.Documents);
        }

        [Fact]
        public void FuzzyMatchesMisspelledTokenWithBoostedScore()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolote", isFuzzy: true));

            Assert.Equal(new[] { "chocolate-bar", "chocolate-cookies" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            Assert.All(result.Documents, x => Assert.Equal(TokenMatching.Default.GetBoost(1, isPrefix: false), x.Score));
        }

        [Fact]
        public void FuzzyUsesEveryCandidateWithinMaximumDistance()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cookias", isFuzzy: true));

            Assert.Equal(new[] { "chocolate-cookies", "slow-cookers" }, result.Documents.Select(x => x.Key));
        }

        [Fact]
        public void FuzzyKeepsExactMatchesAboveSimilarWords()
        {
            var (index, mapper) = CreateIndex();

            using var exact = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cookies"));
            using var fuzzy = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cookies", isFuzzy: true));

            Assert.Equal("chocolate-cookies", Assert.Single(exact.Documents).Key);
            Assert.Collection(
                fuzzy.Documents,
                x =>
                {
                    Assert.Equal("chocolate-cookies", x.Key);
                    Assert.Equal(1f, x.Score);
                },
                x =>
                {
                    Assert.Equal("slow-cookers", x.Key);
                    Assert.Equal(TokenMatching.Default.GetBoost(2, isPrefix: false), x.Score);
                });
        }

        [Fact]
        public void FuzzyDoesNotExpandWordsWithoutSimilarWords()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "cable", isFuzzy: true));

            Assert.Equal("cable-ties", Assert.Single(result.Documents).Key);
        }

        [Theory]
        [InlineData("choc", PrefixMode.None, new string[0])]
        [InlineData("choc", PrefixMode.Last, new[] { "chocolate-bar", "chocolate-cookies" })]
        [InlineData("choc", PrefixMode.All, new[] { "chocolate-bar", "chocolate-cookies" })]
        [InlineData("choc ", PrefixMode.Last, new string[0])]
        [InlineData("choc ", PrefixMode.All, new[] { "chocolate-bar", "chocolate-cookies" })]
        [InlineData("choc bar", PrefixMode.Last, new string[0])]
        [InlineData("choc bar", PrefixMode.All, new[] { "chocolate-bar" })]
        [InlineData("chocolate ba", PrefixMode.Last, new[] { "chocolate-bar" })]
        public void PrefixModeSelectsCompletedWords(string text, PrefixMode prefixMode, string[] expected)
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, text, prefixMode: prefixMode));

            Assert.Equal(expected, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
        }

        [Fact]
        public void PrefixWithoutFuzzyRequiresExactPrefix()
        {
            var (index, mapper) = CreateIndex();

            using var exact = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chox", prefixMode: PrefixMode.Last));
            using var fuzzy = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chox", isFuzzy: true, prefixMode: PrefixMode.Last));

            Assert.Empty(exact.Documents);
            Assert.Equal(new[] { "chocolate-bar", "chocolate-cookies" }, fuzzy.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
        }

        [Fact]
        public void PrefixRanksExactTokenAboveCompletion()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red", prefixMode: PrefixMode.Last));

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
        public void FuzzyScoresVariantLikeExactMatch()
        {
            var (index, mapper) = CreateIndex(new ExpansionMapper(new[] { new KeyValuePair<string, string>("colour", "color") }));

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "colour", isFuzzy: true));

            var document = Assert.Single(result.Documents);

            Assert.Equal("color-pencils", document.Key);
            Assert.Equal(1f, document.Score);
        }

        [Fact]
        public void VariantsAreUsedOnlyWithFuzzy()
        {
            var (index, mapper) = CreateIndex(new ExpansionMapper(new[] { new KeyValuePair<string, string>("colour", "color") }));

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "colour", prefixMode: PrefixMode.Last));

            Assert.Empty(result.Documents);
        }

        [Fact]
        public void FuzzyCombinesCorrectedAndExactTerms()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolote cookies", isFuzzy: true));

            var document = Assert.Single(result.Documents);

            Assert.Equal("chocolate-cookies", document.Key);
            Assert.Equal(1f + TokenMatching.Default.GetBoost(1, isPrefix: false), document.Score);
        }

        [Fact]
        public void PrefixCompletesWordsReplacedBySynonyms()
        {
            var (index, mapper) = CreateIndex(new ExpansionMapper(synonyms: new[] { new ExplicitMappingSynonym(new[] { "red" }, new[] { "cherry" }) }));

            using var exact = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red"));
            using var prefix = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red", prefixMode: PrefixMode.Last));

            Assert.DoesNotContain("redwood-table", exact.Documents.Select(x => x.Key));
            Assert.Equal(exact.Documents.Select(x => x.Key).Append("redwood-table").OrderBy(x => x, StringComparer.Ordinal), prefix.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
        }

        [Fact]
        public void FuzzyMatchesWordsReplacedBySynonyms()
        {
            var (index, mapper) = CreateIndex(new ExpansionMapper(synonyms: new[] { new ExplicitMappingSynonym(new[] { "colour" }, new[] { "hue" }) }));

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "colour", isFuzzy: true));

            Assert.Equal("color-pencils", Assert.Single(result.Documents).Key);
        }

        [Fact]
        public void FuzzyDoesNotExpandMultiWordSynonyms()
        {
            var (index, mapper) = CreateIndex(new ExpansionMapper(synonyms: new[] { new ExplicitMappingSynonym(new[] { "crock pot" }, new[] { "slow cooker" }) }));

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "crock pot", isFuzzy: true));

            Assert.Equal("slow-cookers", Assert.Single(result.Documents).Key);
        }

        [Fact]
        public void TokenExpansionRequiresFieldTokenizer()
        {
            var expansion = new TokenExpansion(new Analyzer(new WhitespaceTokenizer(), new LowerInvariantTokenFilter()));

            Assert.Throws<ArgumentException>(() => new TextField<ExpansionDocument>(x => x.Text, new PorterAnalyzer(), tokenExpansion: expansion));
        }

        [Fact]
        public void FuzzyAndPrefixRequireTokenExpansion()
        {
            var field = new TextField<ExpansionDocument>(x => x.Text, new PorterAnalyzer());

            Assert.Throws<ArgumentException>(() => new TextSearchExpression(field, SearchMode.All, "red", isFuzzy: true));
            Assert.Throws<ArgumentException>(() => new TextSearchExpression(field, SearchMode.All, "red", prefixMode: PrefixMode.Last));
        }

        [Fact]
        public void IllegalPrefixModeIsRejected()
        {
            var (_, mapper) = CreateIndex();

            Assert.Throws<ArgumentException>(() => new TextSearchExpression(mapper.Text, SearchMode.All, "red", prefixMode: (PrefixMode)42));
        }

        private static (Index<ExpansionDocument> Index, ExpansionMapper Mapper) CreateIndex(ExpansionMapper? mapper = null)
        {
            mapper ??= new ExpansionMapper();

            return (IndexBuilder.Build(ExpansionMapper.Documents, mapper, new InstanceTokenEncoderStore()), mapper);
        }
    }
}
