# Your First Index and Query

This example builds a catalog of three products, searches text, filters by price and brand, and retrieves facets. It uses only the `Clara` core; `StandardAnalyzer` normalizes case but does not reduce inflected words to a common form.

## Running the example

From the repository root, create a helper project in the ignored `artifacts` directory:

```powershell
dotnet new console -n ClaraQuickStart -o artifacts/ClaraQuickStart -f net10.0
dotnet add artifacts/ClaraQuickStart/ClaraQuickStart.csproj reference src/Clara/Clara.csproj
```

Replace the generated `Program.cs` with the code below. The example uses a simplified data model. If the project inherits the repository's full analyzer rules, run the demonstration with analyzers disabled for this invocation:

```powershell
dotnet run --project artifacts/ClaraQuickStart -c Release -p:RunAnalyzers=false
```

## Complete example

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Clara;
using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;

namespace ClaraQuickStart
{
    internal static class Program
    {
        private static void Main()
        {
            var products = new[]
            {
                new Product("1", "Red mug", "Alpha", 25m),
                new Product("2", "Blue mug", "Beta", 30m),
                new Product("3", "Green plate", "Alpha", 40m),
            };

            var mapper = new ProductMapper();
            var index = IndexBuilder.Build(
                products, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q
                .Search(mapper.Text, SearchMode.All, "mug")
                .Filter(mapper.Brand, FilterMode.Any, "Alpha")
                .Filter(mapper.Price, from: 10m, to: 35m)
                .Facet(mapper.Brand)
                .Facet(mapper.Price)
                .Sort(mapper.Price, SortDirection.Ascending));

            foreach (var hit in result.Documents)
            {
                Console.WriteLine($"{hit.Key}: {hit.Document.Title}");
            }

            foreach (var value in result.Facets.Field(mapper.Brand).Values)
            {
                Console.WriteLine($"{value.Value}: {value.Count}");
            }

            var prices = result.Facets.Field(mapper.Price);
            Console.WriteLine($"Range: {prices.MinValue} - {prices.MaxValue}");

            // Materialize the page before disposing the result.
            var page = result.Documents.Take(10).Select(x => x.Document).ToArray();
            Console.WriteLine($"Page item count: {page.Length}");
        }
    }

    internal sealed record Product(string Id, string Title, string Brand, decimal Price);

    internal sealed class ProductMapper : IIndexMapper<Product>
    {
        public TextField<Product> Text { get; } =
            new(x => x.Title, new StandardAnalyzer());

        public KeywordField<Product> Brand { get; } =
            new(x => x.Brand, isFilterable: true, isFacetable: true);

        public DecimalField<Product> Price { get; } =
            new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);

        public IEnumerable<Field> GetFields()
        {
            yield return this.Text;
            yield return this.Brand;
            yield return this.Price;
        }

        public string GetDocumentKey(Product item)
        {
            return item.Id;
        }

        public Product GetDocument(Product item)
        {
            return item;
        }
    }
}
```

## Interpreting the results

The document list contains only `1: Red mug`. The brand facet shows `Alpha: 1` and `Beta: 1` because `FilterMode.Any` excludes the brand's own filter from that facet while retaining the search and price filter. The price facet returns `25 - 25`: it excludes its own price filter but respects the Alpha brand filter and the search term "mug".

`Take(10)` only limits reading from the completed result. The engine has already computed all matching documents and facets. Objects copied into `page` can be used after `result` is disposed; do not retain a lazy enumeration or facet views in the same way.

For a larger repository example with assertions, see `ComplexQuery` in [QueryTests.cs](../src/Clara.Tests/QueryTests.cs) and [ProductMapper.cs](../src/Clara.Tests/ProductMapper.cs).
