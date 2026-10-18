#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class IncludeTests
    {
        [Theory]
        [InlineData("A")]
        [InlineData("red")]
        [InlineData("unknown")]
        [InlineData("")]
        [InlineData(" ")]
        public void IncludeRejectsValuesThatAreNotDocumentKeys(string key)
        {
            var mapper = new ProductMapper();
            var index = IndexBuilder.Build(CreateProducts(), mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Include(new[] { key }).Facet(mapper.Brand));

            Assert.Empty(result.Documents);
            Assert.Empty(result.Facets.Field(mapper.Brand).Values);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IncludeUsesOnlyTheCurrentIndexDocuments(bool sharedStore)
        {
            var mapper = new ProductMapper();
            TokenEncoderStore store = sharedStore ? new SharedTokenEncoderStore() : new InstanceTokenEncoderStore();
            var previousIndex = IndexBuilder.Build(new[] { new Product { Id = 99, Title = "removed" } }, mapper, store);
            var index = IndexBuilder.Build(CreateProducts(), mapper, store);

            using var previousResult = previousIndex.Query(q => q.Include(new[] { "99" }));
            using var result = index.Query(q => q.Include(new[] { "99" }));

            Assert.Equal("99", Assert.Single(previousResult.Documents).Key);
            Assert.Empty(result.Documents);
        }

        [Fact]
        public void IncludeIgnoresInvalidKeysAndDuplicatesInMixedSelection()
        {
            var mapper = new ProductMapper();
            var products = CreateProducts();
            var index = IndexBuilder.Build(products, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q
                .Include(new string?[] { "2", "unknown", "A", "red", null, "2" })
                .Facet(mapper.Brand));

            var document = Assert.Single(result.Documents);

            Assert.Equal("2", document.Key);
            Assert.Same(products[1], document.Document);

            var brand = Assert.Single(result.Facets.Field(mapper.Brand).Values);

            Assert.Equal("A", brand.Value);
            Assert.Equal(1, brand.Count);
        }

        [Fact]
        public void EmptyIncludeDoesNotRestrictDocuments()
        {
            var mapper = new ProductMapper();
            var products = CreateProducts();
            var index = IndexBuilder.Build(products, mapper, new InstanceTokenEncoderStore());
            var selections = new IEnumerable<string?>?[]
            {
                null,
                Array.Empty<string?>(),
                new string?[] { null, null },
            };

            foreach (var selection in selections)
            {
                using var result = index.Query(q => q.Include(selection));

                Assert.Equal(products.Length, result.Documents.Count);
                Assert.Equal(new[] { "1", "2", "3", "4" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            }
        }

        [Fact]
        public void IncludeCombinesWithSearchFiltersFacetsAndExclusion()
        {
            var mapper = new ProductMapper();
            var index = IndexBuilder.Build(CreateProducts(), mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q
                .Include(new[] { "1", "2", "3", "4", "A", "red", "unknown" })
                .Search(mapper.Text, SearchMode.All, "red")
                .Filter(mapper.Brand, FilterMode.Any, "A")
                .Exclude(new[] { "1" })
                .Facet(mapper.Brand));

            Assert.Equal("2", Assert.Single(result.Documents).Key);

            var brands = result.Facets.Field(mapper.Brand).Values;

            Assert.Equal(2, brands.Count);
            Assert.Equal(1, brands.Single(x => x.Value == "A").Count);
            Assert.Equal(1, brands.Single(x => x.Value == "B").Count);
        }

        private static Product[] CreateProducts()
        {
            return new[]
            {
                new Product { Id = 1, Title = "red", Brand = "A" },
                new Product { Id = 2, Title = "red", Brand = "A" },
                new Product { Id = 3, Title = "red", Brand = "B" },
                new Product { Id = 4, Title = "blue", Brand = "A" },
            };
        }
    }
}
