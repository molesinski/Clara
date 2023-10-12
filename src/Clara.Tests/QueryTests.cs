using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class QueryTests
    {
        private const string AllTextPhrase = "__ALL";
        private const string NoneTextPhrase = "__NONE";
        private const string AllNoneTextPhrase = "__ALL __NONE";

        private readonly string?[] topBrands;
        private readonly decimal? maxPrice;
        private readonly Index<Product> index;

        public QueryTests()
        {
            this.topBrands = Product.Items
                .Select(x => x.Brand)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .ToArray();

            this.maxPrice = Product.Items
                .Max(x => x.Price);

            var synonymMapBindings =
                new[]
                {
                    new SynonymMapBinding(
                        new SynonymMap(
                            ProductMapper.Text.Analyzer,
                            new Synonym[]
                            {
                                new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, AllTextPhrase }),
                            }),
                        ProductMapper.Text),
                };

            this.index = IndexBuilder.Build(Product.Items, new ProductMapper(), synonymMapBindings);
        }

        [Fact]
        public void ComplexQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, AllNoneTextPhrase)
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrands[0])
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Price, SortDirection.Descending));

            var input = new HashSet<Product>(
                Product.Items
                    .Where(x => x.Brand == this.topBrands[0])
                    .Where(x => x.Price >= 1 && x.Price <= this.maxPrice - 1));

            var output = new HashSet<Product>(
                result.Documents
                    .Select(x => x.Document));

            Assert.True(input.SetEquals(output));

            var inputSorted = input.OrderByDescending(x => x.Price).Select(x => x.Price);
            var outputSorted = result.Documents.Select(x => x.Document.Price);

            Assert.True(inputSorted.SequenceEqual(outputSorted));

            var brandFacet = result.Facets.Field(ProductMapper.Brand);

            Assert.True(brandFacet.Values.Count > 1);
            Assert.True(brandFacet.Values.Count(x => x.IsSelected) == 1);
            Assert.True(brandFacet.Values.Single(x => x.IsSelected).Value == this.topBrands[0]);

            var priceFacet = result.Facets.Field(ProductMapper.Price);

            var minBrandPrice = input
                .Where(x => x.Brand == this.topBrands[0])
                .Min(x => x.Price);

            var maxBrandPrice = input
                .Where(x => x.Brand == this.topBrands[0])
                .Max(x => x.Price);

            Assert.True(priceFacet.Min == minBrandPrice);
            Assert.True(priceFacet.Max == maxBrandPrice);
        }

        [Theory]
        [InlineData(default(string?), SearchMode.All, 100)]
        [InlineData(default(string?), SearchMode.Any, 100)]
        [InlineData("", SearchMode.Any, 100)]
        [InlineData("", SearchMode.All, 100)]
        [InlineData(" ", SearchMode.Any, 100)]
        [InlineData(" ", SearchMode.All, 100)]
        [InlineData("...", SearchMode.Any, 0)]
        [InlineData("...", SearchMode.All, 0)]
        [InlineData(NoneTextPhrase, SearchMode.Any, 0)]
        [InlineData(NoneTextPhrase, SearchMode.All, 0)]
        [InlineData(AllTextPhrase, SearchMode.Any, 100)]
        [InlineData(AllTextPhrase, SearchMode.All, 100)]
        [InlineData(AllNoneTextPhrase, SearchMode.Any, 100)]
        [InlineData(AllNoneTextPhrase, SearchMode.All, 0)]
        public void SearchQuery(string? text, SearchMode mode, int expectedCount)
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, mode, text));

            Assert.Equal(expectedCount, result.Documents.Count);
        }

        [Theory]
        [InlineData(-2, FilterMode.All, 100)]
        [InlineData(-2, FilterMode.Any, 100)]
        [InlineData(-1, FilterMode.All, 100)]
        [InlineData(-1, FilterMode.Any, 100)]
        [InlineData(0, FilterMode.All, 100)]
        [InlineData(0, FilterMode.Any, 100)]
        [InlineData(1, FilterMode.All, -1)]
        [InlineData(1, FilterMode.Any, -1)]
        [InlineData(2, FilterMode.All, 0)]
        [InlineData(2, FilterMode.Any, -1)]
        public void FilterQuery(int count, FilterMode mode, int expectedCount)
        {
            var values =
                count switch
                {
                    -2 => default(IEnumerable<string>),
                    -1 => new[] { default(string) },
                    0 => Array.Empty<string>(),
                    _ => this.topBrands.Take(count),
                };

            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Filter(ProductMapper.Brand, mode, values));

            if (expectedCount < 0)
            {
                var valuesSet = new HashSet<string?>(
                    count <= 0
                        ? Array.Empty<string>()
                        : this.topBrands.Take(count));

                expectedCount =
                    mode switch
                    {
                        FilterMode.All => Product.Items.Where(x => valuesSet.SetEquals(new[] { x.Brand })).Count(),
                        _ => Product.Items.Where(x => valuesSet.IsSupersetOf(new[] { x.Brand })).Count(),
                    };
            }

            Assert.Equal(expectedCount, result.Documents.Count);
        }
    }
}
