#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class PhraseSuggesterTests
    {
        [Fact]
        public void BuildRemovesPhrasesWithoutDocuments()
        {
            var (_, _, suggester) = CreateSuggester();

            Assert.Equal(5, suggester.Count);
        }

        [Fact]
        public void EmptyTextReturnsPhrasesByWeight()
        {
            var (index, _, suggester) = CreateSuggester();

            using var result = index.Query(q => q.Suggest(suggester, " ", count: 10));

            Assert.Equal(
                new[] { "chocolate", "chocolate cookies", "red apple", "cherry juice", "redwood table" },
                result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestCompletesPhrasesAndHighlightsCompletion()
        {
            var (index, _, suggester) = CreateSuggester();

            using var result = index.Query(q => q.Suggest(suggester, "choc", count: 10));

            Assert.Collection(
                result.Suggestions,
                x =>
                {
                    Assert.Equal("chocolate", x.Text);
                    Assert.Equal("choc<b>olate</b>", x.HighlightedText);
                    Assert.Equal(999_995, x.Score);
                },
                x =>
                {
                    Assert.Equal("chocolate cookies", x.Text);
                    Assert.Equal("choc<b>olate</b> <b>cookies</b>", x.HighlightedText);
                    Assert.Equal(999_994, x.Score);
                });
        }

        [Fact]
        public void SuggestRanksExactWordAboveCompletion()
        {
            var (index, _, suggester) = CreateSuggester();

            using var result = index.Query(q => q.Suggest(suggester, "red", count: 10));

            Assert.Equal(new[] { "red apple", "redwood table" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestCorrectsMisspelledWords()
        {
            var (index, _, suggester) = CreateSuggester();

            using var result = index.Query(q => q.Suggest(suggester, "chocolote ", count: 10));

            Assert.Equal(new[] { "chocolate", "chocolate cookies" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestMatchesVariantPrefixes()
        {
            var (index, _, suggester) = CreateSuggester(new[] { new KeyValuePair<string, string>("biscuits", "cookies") });

            using var result = index.Query(q => q.Suggest(suggester, "chocolate biscu", count: 10));

            Assert.Equal("chocolate cookies", Assert.Single(result.Suggestions).Text);
        }

        [Fact]
        public void SuggestRespectsFilters()
        {
            var (index, mapper, suggester) = CreateSuggester();

            using var completion = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "alpha").Suggest(suggester, "choc", count: 10));
            using var popular = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "alpha").Suggest(suggester, null, count: 10));

            Assert.Equal(new[] { "chocolate" }, completion.Suggestions.Select(x => x.Text));
            Assert.Equal(new[] { "chocolate", "cherry juice" }, popular.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestRespectsSearchAndExcludedDocuments()
        {
            var (index, mapper, suggester) = CreateSuggester();

            using var result =
                index.Query(
                    q => q
                        .Search(mapper.Text, SearchMode.All, "chocolate")
                        .Exclude(new[] { "chocolate-bar" })
                        .Suggest(suggester, null, count: 10));

            Assert.Equal(new[] { "chocolate", "chocolate cookies" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestLimitsCount()
        {
            var (index, _, suggester) = CreateSuggester();

            using var result = index.Query(q => q.Suggest(suggester, null, count: 2));

            Assert.Equal(2, result.Suggestions.Count);
        }

        [Fact]
        public void SuggestRejectsSuggesterBuiltForAnotherIndex()
        {
            var (_, _, suggester) = CreateSuggester();
            var (otherIndex, _, _) = CreateSuggester();

            using var builder = otherIndex.QueryBuilder();

            Assert.Throws<InvalidOperationException>(() => builder.Suggest(suggester, "choc"));
        }

        [Fact]
        public void ConcurrentSuggestionsMatchSequentialResults()
        {
            var (index, mapper, suggester) = CreateSuggester();
            var queries = new[] { "choc", "red", "chocolote ", "cherry j", "redwood t", "chocolate c" };
            var expected = queries.ToDictionary(x => x, x => Suggest(x));

            Parallel.For(
                0,
                2_000,
                i =>
                {
                    var query = queries[i % queries.Length];

                    Assert.Equal(expected[query], Suggest(query));
                });

            string Suggest(string text)
            {
                using var result = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "alpha", "beta", "gamma").Suggest(suggester, text, count: 10));

                return string.Join("|", result.Suggestions.Select(x => x.HighlightedText));
            }
        }

        private static (Index<CorrectionDocument> Index, CorrectionMapper Mapper, PhraseSuggester Suggester) CreateSuggester(IEnumerable<KeyValuePair<string, string>>? variants = null)
        {
            var mapper = new CorrectionMapper();
            var index = IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore());
            var builder = new PhraseSuggesterBuilder(index, mapper.Text, CorrectionMapper.SurfaceAnalyzer, variants: variants);

            builder.Add("unicorn", 100);
            builder.Add("chocolate", 50);
            builder.Add("chocolate cookies", 30);
            builder.Add("red apple", 20);
            builder.Add("cherry juice", 10);
            builder.Add("redwood table", 5);

            return (index, mapper, builder.Build());
        }
    }
}
