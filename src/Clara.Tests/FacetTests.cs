#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class FacetTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void RangeAndKeywordFacetsIgnoreOnlyTheirOwnFilters(bool priceFirst)
        {
            var mapper = new FacetMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new FacetDocument("a10", "A", 10),
                    new FacetDocument("a20", "A", 20),
                    new FacetDocument("b15", "B", 15),
                    new FacetDocument("b30", "B", 30),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q =>
            {
                if (priceFirst)
                {
                    q.Filter(mapper.Price, from: null, to: 15m).Filter(mapper.Brand, FilterMode.Any, "A");
                }
                else
                {
                    q.Filter(mapper.Brand, FilterMode.Any, "A").Filter(mapper.Price, from: null, to: 15m);
                }

                q.Facet(mapper.Brand).Facet(mapper.Price);
            });

            Assert.Equal("a10", Assert.Single(result.Documents).Key);
            AssertKeywordValues(result.Facets.Field(mapper.Brand), ("A", 1, true), ("B", 1, false));
            var price = result.Facets.Field(mapper.Price);
            Assert.Equal(10m, price.MinValue);
            Assert.Equal(20m, price.MaxValue);
        }

        [Theory]
        [InlineData(FilterMode.Any)]
        [InlineData(FilterMode.All)]
        public void KeywordFacetHonorsFilterModeAndCountsEachDocumentOnce(FilterMode mode)
        {
            var mapper = new FacetMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new FacetDocument("both", "A", 10, "red", "red", "blue"),
                    new FacetDocument("red", "A", 10, "red"),
                    new FacetDocument("blue", "A", 10, "blue"),
                    new FacetDocument("green", "A", 10, "green", "green"),
                    new FacetDocument("expensive", "A", 100, "red", "blue", "green"),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q
                .Filter(mapper.Tags, mode, "red", "blue")
                .Filter(mapper.Price, from: null, to: 15m)
                .Facet(mapper.Tags));

            var facet = result.Facets.Field(mapper.Tags);

            if (mode == FilterMode.Any)
            {
                Assert.Equal(new[] { "blue", "both", "red" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
                AssertKeywordValues(facet, ("red", 2, true), ("blue", 2, true), ("green", 1, false));
                Assert.All(facet.Values.Take(2), x => Assert.True(x.IsSelected));
                Assert.False(facet.Values[2].IsSelected);
            }
            else
            {
                Assert.Equal("both", Assert.Single(result.Documents).Key);
                AssertKeywordValues(facet, ("red", 1, true), ("blue", 1, true));
            }
        }

        [Theory]
        [InlineData(FilterMode.Any)]
        [InlineData(FilterMode.All)]
        public void KeywordFacetRetainsUnknownSelectedValueWithZeroCount(FilterMode mode)
        {
            var mapper = new FacetMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new FacetDocument("a1", "A", 10),
                    new FacetDocument("a2", "A", 20),
                    new FacetDocument("b", "B", 15),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Brand, mode, "missing").Facet(mapper.Brand));

            Assert.Empty(result.Documents);
            var facet = result.Facets.Field(mapper.Brand);

            if (mode == FilterMode.Any)
            {
                AssertKeywordValues(facet, ("missing", 0, true), ("A", 2, false), ("B", 1, false));
                Assert.Equal(new[] { "missing", "A", "B" }, facet.Values.Select(x => x.Value));
            }
            else
            {
                AssertKeywordValues(facet, ("missing", 0, true));
            }
        }

        [Fact]
        public void KeywordFacetRetainsSelectedValueExcludedByAnotherFilter()
        {
            var mapper = new FacetMapper();
            var index = IndexBuilder.Build(
                new[] { new FacetDocument("a", "A", 20), new FacetDocument("b", "B", 10) },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q
                .Filter(mapper.Brand, FilterMode.Any, "A")
                .Filter(mapper.Price, from: null, to: 15m)
                .Facet(mapper.Brand)
                .Facet(mapper.Price));

            Assert.Empty(result.Documents);
            AssertKeywordValues(result.Facets.Field(mapper.Brand), ("A", 0, true), ("B", 1, false));
            Assert.Equal(20m, result.Facets.Field(mapper.Price).MinValue);
            Assert.Equal(20m, result.Facets.Field(mapper.Price).MaxValue);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void KeywordFacetResultsDoNotDependOnFilterInsertionOrder(bool tagsFirst)
        {
            var mapper = new FacetMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new FacetDocument("ar", "A", 10, "red"),
                    new FacetDocument("ab", "A", 20, "blue"),
                    new FacetDocument("br", "B", 15, "red"),
                    new FacetDocument("bb", "B", 30, "blue"),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q =>
            {
                if (tagsFirst)
                {
                    q.Filter(mapper.Tags, FilterMode.Any, "red").Filter(mapper.Brand, FilterMode.Any, "A");
                }
                else
                {
                    q.Filter(mapper.Brand, FilterMode.Any, "A").Filter(mapper.Tags, FilterMode.Any, "red");
                }

                q.Facet(mapper.Brand).Facet(mapper.Tags);
            });

            Assert.Equal("ar", Assert.Single(result.Documents).Key);
            AssertKeywordValues(result.Facets.Field(mapper.Brand), ("A", 1, true), ("B", 1, false));
            AssertKeywordValues(result.Facets.Field(mapper.Tags), ("red", 1, true), ("blue", 1, false));
        }

        [Fact]
        public void FacetsRespectSearchIncludeAndExclude()
        {
            var mapper = new FacetMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new FacetDocument("low", "A", 10),
                    new FacetDocument("high", "A", 20),
                    new FacetDocument("alternative", "B", 5),
                    new FacetDocument("outside-high", "A", 40),
                    new FacetDocument("outside-low", "B", 8),
                    new FacetDocument("excluded-high", "A", 30),
                    new FacetDocument("excluded-low", "B", 8),
                    new FacetDocument("unmatched-high", "A", 50) { Text = "blue" },
                    new FacetDocument("unmatched-low", "B", 8) { Text = "blue" },
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q
                .Search(mapper.Text, SearchMode.All, "red")
                .Include(new[] { "low", "high", "alternative", "excluded-high", "excluded-low", "unmatched-high", "unmatched-low" })
                .Exclude(new[] { "excluded-high", "excluded-low" })
                .Filter(mapper.Brand, FilterMode.Any, "A")
                .Filter(mapper.Price, from: null, to: 15m)
                .Facet(mapper.Brand)
                .Facet(mapper.Price));

            Assert.Equal("low", Assert.Single(result.Documents).Key);
            AssertKeywordValues(result.Facets.Field(mapper.Brand), ("A", 1, true), ("B", 1, false));
            Assert.Equal(10m, result.Facets.Field(mapper.Price).MinValue);
            Assert.Equal(20m, result.Facets.Field(mapper.Price).MaxValue);
        }

        private static void AssertKeywordValues(KeywordFacetResult facet, params (string Value, int Count, bool IsSelected)[] expected)
        {
            Assert.Equal(
                expected.OrderBy(x => x.Value, StringComparer.Ordinal),
                facet.Values.Select(x => (x.Value, x.Count, x.IsSelected)).OrderBy(x => x.Value, StringComparer.Ordinal));
        }

        private sealed class FacetDocument
        {
            public FacetDocument(string key, string brand, decimal price, params string[] tags)
            {
                this.Key = key;
                this.Brand = brand;
                this.Price = price;
                this.Tags = tags;
            }

            public string Key { get; }

            public string Brand { get; }

            public decimal Price { get; }

            public string[] Tags { get; }

            public string Text { get; set; } = "red";
        }

        private sealed class FacetMapper : IIndexMapper<FacetDocument>
        {
            public TextField<FacetDocument> Text { get; } = new(x => x.Text, new StandardAnalyzer());

            public KeywordField<FacetDocument> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);

            public KeywordField<FacetDocument> Tags { get; } = new(x => x.Tags, isFilterable: true, isFacetable: true);

            public DecimalField<FacetDocument> Price { get; } = new(x => x.Price, isFilterable: true, isFacetable: true);

            public IEnumerable<Field> GetFields()
            {
                yield return this.Text;
                yield return this.Brand;
                yield return this.Tags;
                yield return this.Price;
            }

            public string GetDocumentKey(FacetDocument item)
            {
                return item.Key;
            }

            public FacetDocument GetDocument(FacetDocument item)
            {
                return item;
            }
        }
    }
}
