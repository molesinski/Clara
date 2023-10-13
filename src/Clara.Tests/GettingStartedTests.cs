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
            var mapper = new ProductMapper();
            var index = IndexBuilder.Build(Product.Items, mapper);

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(mapper.Text, SearchMode.Any, "watch ring leather bag")
                    .Filter(mapper.Brand, FilterMode.Any, "Eastern Watches", "Bracelet", "Copenhagen Luxe")
                    .Filter(mapper.Category, FilterMode.Any, "womens")
                    .Filter(mapper.Price, from: 10, to: 90)
                    .Facet(mapper.Brand)
                    .Facet(mapper.Category)
                    .Facet(mapper.Price)
                    .Sort(mapper.Price, SortDirection.Descending));

            this.output.WriteLine("Documents:");

            foreach (var document in result.Documents.Take(10))
            {
                this.output.WriteLine($"  [{document.Document.Title}] ${document.Document.Price} => {document.Score}");
            }

            this.output.WriteLine("Brands:");

            foreach (var value in result.Facets.Field(mapper.Brand).Values.Take(5))
            {
                this.output.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
            }

            this.output.WriteLine("Categories:");

            foreach (var value in result.Facets.Field(mapper.Category).Values.Take(5))
            {
                this.output.WriteLine($"  (x) [{value.Value}] => {value.Count}");

                foreach (var child in value.Children)
                {
                    this.output.WriteLine($"    ( ) [{child.Value}] => {child.Count}");
                }
            }

            var priceFacet = result.Facets.Field(mapper.Price);

            this.output.WriteLine("Price:");
            this.output.WriteLine($"  [Min] => {priceFacet.Min}");
            this.output.WriteLine($"  [Max] => {priceFacet.Max}");
        }
    }
}
