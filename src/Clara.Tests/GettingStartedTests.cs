using Clara.Querying;
using Clara.Storage;
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
            var sharedTokenEncoderStore = new SharedTokenEncoderStore();

            var index = IndexBuilder.Build(Product.Items, new ProductMapper(), sharedTokenEncoderStore);

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, "watch ring leather bag")
                    .Filter(ProductMapper.Brand, FilterMode.Any, "Eastern Watches", "Bracelet", "Copenhagen Luxe")
                    .Filter(ProductMapper.Category, FilterMode.Any, "womens")
                    .Filter(ProductMapper.Price, from: 10, to: 90)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Price, SortDirection.Descending));

            this.output.WriteLine("Documents:");

            foreach (var document in result.Documents.Take(10))
            {
                this.output.WriteLine($"  [{document.Document.Title}] ${document.Document.Price} => {document.Score}");
            }

            this.output.WriteLine("Brands:");

            foreach (var value in result.Facets.Field(ProductMapper.Brand).Values.Take(5))
            {
                this.output.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
            }

            this.output.WriteLine("Categories:");

            foreach (var value in result.Facets.Field(ProductMapper.Category).Values.Take(5))
            {
                this.output.WriteLine($"  (x) [{value.Value}] => {value.Count}");

                foreach (var child in value.Children)
                {
                    this.output.WriteLine($"    ( ) [{child.Value}] => {child.Count}");
                }
            }

            var priceFacet = result.Facets.Field(ProductMapper.Price);

            this.output.WriteLine("Price:");
            this.output.WriteLine($"  [Min] => {priceFacet.Min}");
            this.output.WriteLine($"  [Max] => {priceFacet.Max}");
        }
    }
}
