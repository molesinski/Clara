#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class PhraseFieldTests
    {
        [Fact]
        public void ValidationDropsPairsWhosePhraseDoesNotFindTheDocument()
        {
            var (index, mapper) = CreateIndex();

            using var completion = index.Query(q => q.Suggest(mapper.Phrases, "uni", count: 10));
            using var popular = index.Query(q => q.Suggest(mapper.Phrases, null, count: 100));

            Assert.Empty(completion.Suggestions);
            Assert.DoesNotContain("unicorn", popular.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void PairsAreKeptWithoutValidationField()
        {
            var (index, mapper) = CreateIndex(new CorrectionMapper(validatePhrases: false));

            using var result = index.Query(q => q.Suggest(mapper.Phrases, "uni", count: 10));

            Assert.Equal("unicorn", Assert.Single(result.Suggestions).Text);
        }

        [Fact]
        public void EmptyTextReturnsPhrasesByWeightAggregatedAcrossDocuments()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Suggest(mapper.Phrases, " ", count: 10));

            Assert.Equal(
                new[] { "cherry", "chocolate", "chocolate cookies", "red apple", "cherry juice", "chocolate bar", "redwood table", "slow cookers" },
                result.Suggestions.Select(x => x.Text));
            Assert.Equal(new[] { 40f, 30f, 30f, 20f, 10f, 5f, 5f, 0f }, result.Suggestions.Select(x => x.Weight));
        }

        [Fact]
        public void SuggestCompletesPhrasesAndHighlightsCompletion()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Suggest(mapper.Phrases, "choc", count: 10));

            Assert.Equal(new[] { "chocolate", "chocolate cookies", "chocolate bar" }, result.Suggestions.Select(x => x.Text));
            Assert.Equal(new[] { "choc<b>olate</b>", "choc<b>olate</b> <b>cookies</b>", "choc<b>olate</b> <b>bar</b>" }, result.Suggestions.Select(x => x.HighlightedText));
            Assert.Equal(new[] { 999_995, 999_994, 999_994 }, result.Suggestions.Select(x => x.Score));
        }

        [Fact]
        public void SuggestCorrectsMisspelledWords()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Suggest(mapper.Phrases, "chocolote ", count: 10));

            Assert.Equal(new[] { "chocolate", "chocolate cookies", "chocolate bar" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void FiltersRemoveContributionOfExcludedDocuments()
        {
            var (index, mapper) = CreateIndex();

            using var unfiltered = index.Query(q => q.Suggest(mapper.Phrases, "ch", count: 10));
            using var alpha = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "alpha").Suggest(mapper.Phrases, "ch", count: 10));

            Assert.Equal(new[] { "cherry", "chocolate", "chocolate cookies", "cherry juice", "chocolate bar" }, unfiltered.Suggestions.Select(x => x.Text));
            Assert.Equal(new[] { "cherry", "chocolate", "cherry juice", "chocolate bar" }, alpha.Suggestions.Select(x => x.Text));
            Assert.Equal(new[] { 40f, 30f, 10f, 5f }, alpha.Suggestions.Select(x => x.Weight));
        }

        [Fact]
        public void PhraseWithoutWeightUnderFiltersIsPenalized()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "beta").Suggest(mapper.Phrases, "ch", count: 10));

            Assert.Collection(
                result.Suggestions,
                x =>
                {
                    Assert.Equal("chocolate cookies", x.Text);
                    Assert.Equal(999_994, x.Score);
                    Assert.Equal(30f, x.Weight);
                },
                x =>
                {
                    Assert.Equal("chocolate", x.Text);
                    Assert.Equal(999_994, x.Score);
                    Assert.Equal(0f, x.Weight);
                });
        }

        [Theory]
        [InlineData("alpha", new[] { "cherry", "chocolate", "cherry juice", "chocolate bar" })]
        [InlineData("beta", new[] { "chocolate cookies", "chocolate" })]
        public void EmptyTextUnderFiltersUsesFilteredWeights(string brand, string[] expected)
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, brand).Suggest(mapper.Phrases, null, count: 10));

            Assert.Equal(expected, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void EmptyTextUnderFiltersKeepsTopPhrasesWhenCountIsLimited()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "alpha", "gamma").Suggest(mapper.Phrases, null, count: 3));

            Assert.Equal(new[] { "cherry", "chocolate", "red apple" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestRespectsSearchAndExcludedDocuments()
        {
            var (index, mapper) = CreateIndex();

            using var result =
                index.Query(
                    q => q
                        .Search(mapper.Text, SearchMode.All, "chocolate")
                        .Exclude(new[] { "chocolate-bar" })
                        .Suggest(mapper.Phrases, null, count: 10));

            Assert.Equal(new[] { "chocolate cookies", "chocolate" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void SuggestMatchesVariantsAndAliases()
        {
            var (index, mapper) =
                CreateIndex(
                    new CorrectionMapper(
                        phraseVariants: new[] { new KeyValuePair<string, string>("biscuits", "cookies") },
                        phraseAliases: new[] { new KeyValuePair<string, string>("cherry juice", "cherry nectar") }));

            using var variant = index.Query(q => q.Suggest(mapper.Phrases, "chocolate biscu", count: 10));
            using var alias = index.Query(q => q.Suggest(mapper.Phrases, "cherry nec", count: 10));

            Assert.Equal("chocolate cookies", Assert.Single(variant.Suggestions).Text);
            Assert.Equal("cherry juice", Assert.Single(alias.Suggestions).Text);
        }

        [Fact]
        public void CountIsNumberOfDocumentsCarryingPhrase()
        {
            var (index, mapper) = CreateIndex();

            using var completion = index.Query(q => q.Suggest(mapper.Phrases, "ch", count: 10));
            using var popular = index.Query(q => q.Suggest(mapper.Phrases, null, count: 10));

            Assert.Equal(new[] { "cherry", "chocolate", "chocolate cookies", "cherry juice", "chocolate bar" }, completion.Suggestions.Select(x => x.Text));
            Assert.Equal(new[] { 1, 2, 1, 1, 1 }, completion.Suggestions.Select(x => x.Count));
            Assert.Equal(2, popular.Suggestions.Single(x => x.Text == "chocolate").Count);
            Assert.All(popular.Suggestions.Where(x => x.Text != "chocolate"), x => Assert.Equal(1, x.Count));
        }

        [Fact]
        public void CountIncludesOnlyRemainingDocuments()
        {
            var (index, mapper) = CreateIndex();

            using var filtered = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "alpha", "beta").Suggest(mapper.Phrases, "chocolate", count: 10));
            using var searched = index.Query(q => q.Search(mapper.Text, SearchMode.All, "chocolate").Exclude(new[] { "chocolate-bar" }).Suggest(mapper.Phrases, null, count: 10));
            using var empty = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "beta").Suggest(mapper.Phrases, null, count: 10));

            Assert.Equal(2, filtered.Suggestions.Single(x => x.Text == "chocolate").Count);
            Assert.Equal(new[] { ("chocolate cookies", 1), ("chocolate", 1) }, searched.Suggestions.Select(x => (x.Text, x.Count)));
            Assert.Equal(new[] { ("chocolate cookies", 30f, 1), ("chocolate", 0f, 1) }, empty.Suggestions.Select(x => (x.Text, x.Weight, x.Count)));
        }

        [Fact]
        public void SuggestLimitsCount()
        {
            var (index, mapper) = CreateIndex();

            using var result = index.Query(q => q.Suggest(mapper.Phrases, "ch", count: 2));

            Assert.Equal(new[] { "cherry", "chocolate" }, result.Suggestions.Select(x => x.Text));
        }

        [Fact]
        public void ValidationFieldMustBelongToTheIndex()
        {
            var other = new CorrectionMapper();
            var field = new PhraseField<CorrectionDocument>(x => x.Phrases, CorrectionMapper.SurfaceAnalyzer, validationField: other.Text);

            Assert.Throws<InvalidOperationException>(() => IndexBuilder.Build(CorrectionMapper.Documents, new PhraseOnlyMapper(field), new InstanceTokenEncoderStore()));
        }

        [Fact]
        public void SuggestRejectsFieldOfAnotherIndex()
        {
            var (index, _) = CreateIndex();
            var other = new CorrectionMapper();

            Assert.Throws<InvalidOperationException>(() => index.Query(q => q.Suggest(other.Phrases, "ch")));
        }

        [Fact]
        public void ConcurrentSuggestionsMatchSequentialResults()
        {
            var (index, mapper) = CreateIndex();
            var queries = new[] { "ch", "choc", "red", "chocolote ", "cherry j", "redwood t", "chocolate c", string.Empty };
            var brands = new[] { "alpha", "beta", "gamma" };
            var expected = queries.SelectMany(x => brands.Select(y => (x, y))).ToDictionary(x => x, x => Suggest(x.x, x.y));

            Parallel.For(
                0,
                2_000,
                i =>
                {
                    var key = (queries[i % queries.Length], brands[i % brands.Length]);

                    Assert.Equal(expected[key], Suggest(key.Item1, key.Item2));
                });

            string Suggest(string text, string brand)
            {
                using var result = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, brand).Suggest(mapper.Phrases, text, count: 10));

                return string.Join("|", result.Suggestions.Select(x => $"{x.HighlightedText}:{x.Score}:{x.Weight}"));
            }
        }

        private static (Index<CorrectionDocument> Index, CorrectionMapper Mapper) CreateIndex(CorrectionMapper? mapper = null)
        {
            mapper ??= new CorrectionMapper();

            return (IndexBuilder.Build(CorrectionMapper.Documents, mapper, new InstanceTokenEncoderStore()), mapper);
        }

        private sealed class PhraseOnlyMapper : IIndexMapper<CorrectionDocument>
        {
            private readonly PhraseField<CorrectionDocument> field;

            public PhraseOnlyMapper(PhraseField<CorrectionDocument> field)
            {
                this.field = field;
            }

            public IEnumerable<Field> GetFields()
            {
                yield return this.field;
            }

            public string GetDocumentKey(CorrectionDocument item)
            {
                return item.Key;
            }

            public CorrectionDocument GetDocument(CorrectionDocument item)
            {
                return item;
            }
        }
    }
}
