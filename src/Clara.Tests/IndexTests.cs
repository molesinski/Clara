using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class IndexTests
    {
        [Fact]
        public void IndexAndQuery()
        {
            var allTextSynonym = Guid.NewGuid().ToString("N");

            var topBrand = Product.Items
                .Select(x => x.Brand)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            var maxPrice = Product.Items
                .Max(x => x.Price);

            var synonymMap =
                new SynonymTree(
                    ProductMapper.Text,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, allTextSynonym }),
                    });

            var builder =
                new IndexBuilder<Product, Product>(
                    new ProductMapper(),
                    new[]
                    {
                        synonymMap,
                    });

            foreach (var item in Product.Items)
            {
                builder.Index(item);
            }

            var index = builder.Build();

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(ProductMapper.Text, allTextSynonym)
                    .Filter(ProductMapper.Brand, Values.Any(topBrand))
                    .Filter(ProductMapper.Price, from: 1, to: maxPrice - 1)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Price, SortDirection.Descending));

            var input = Product.Items
                .Where(x => x.Brand == topBrand)
                .Where(x => x.Price >= 1 && x.Price <= maxPrice - 1)
                .ToList();

            var inputSet = new HashSet<int>(input.Select(x => x.Id));
            var outputSet = new HashSet<int>(result.Documents.Select(x => x.Document.Id));

            Assert.True(inputSet.SetEquals(outputSet));

            var inputSorted = input.OrderByDescending(x => x.Price).Select(x => x.Price);
            var outputSorted = result.Documents.Select(x => x.Document.Price);

            Assert.True(inputSorted.SequenceEqual(outputSorted));

            var brandFacet = result.Facets.Field(ProductMapper.Brand);

            Assert.True(brandFacet.Values.Count() > 1);
            Assert.True(brandFacet.Values.Count(x => x.IsSelected) == 1);
            Assert.True(brandFacet.Values.Single(x => x.IsSelected).Value == topBrand);

            var priceFacet = result.Facets.Field(ProductMapper.Price);

            var minBrandPrice = input
                .Where(x => x.Brand == topBrand)
                .Min(x => x.Price);

            var maxBrandPrice = input
                .Where(x => x.Brand == topBrand)
                .Max(x => x.Price);

            Assert.True(priceFacet.Min == minBrandPrice);
            Assert.True(priceFacet.Max == maxBrandPrice);
        }
    }
}
