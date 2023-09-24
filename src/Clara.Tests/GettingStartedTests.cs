using Clara.Querying;
using Xunit;
using Xunit.Abstractions;

namespace Clara.Tests
{
    public class GettingStartedTests
    {
        private readonly ITestOutputHelper output;

        public GettingStartedTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void GettingStarted()
        {
            var builder =
                new IndexBuilder<Product, Product>(
                    new ProductMapper());

            foreach (var item in Product.Items)
            {
                builder.Index(item);
            }

            var index = builder.Build();

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(ProductMapper.Text, "smartphone", SearchMode.Any)
                    .Filter(ProductMapper.Brand, Values.Any("Apple", "Samsung"))
                    .Filter(ProductMapper.Price, from: 300, to: 1500)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Price, SortDirection.Descending));

            this.output.WriteLine("Documents:");

            foreach (var document in result.Documents.Take(10))
            {
                this.output.WriteLine($"  [{document.Document.Title}] => {document.Score}");
            }

            this.output.WriteLine("Brands:");

            foreach (var value in result.Facets.Field(ProductMapper.Brand).Values.Take(5))
            {
                this.output.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
            }

            this.output.WriteLine("Categories:");

            foreach (var value in result.Facets.Field(ProductMapper.Category).Values.Take(5))
            {
                this.output.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
            }

            var priceFacet = result.Facets.Field(ProductMapper.Price);

            this.output.WriteLine("Price:");
            this.output.WriteLine($"  [Min] => {priceFacet.Min}");
            this.output.WriteLine($"  [Max] => {priceFacet.Max}");
        }
    }
}
