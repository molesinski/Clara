# Clara

[![CI](https://dev.azure.com/molesinski/Clara/_apis/build/status/Clara?branchName=master)](https://dev.azure.com/molesinski/Clara/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/dt/clara.svg)](https://www.nuget.org/packages/clara) 
[![NuGet](https://img.shields.io/nuget/v/clara.svg)](https://www.nuget.org/packages/clara)
[![License](https://img.shields.io/github/license/molesinski/clara.svg)](https://github.com/molesinski/clara/blob/master/LICENSE)

Simple, yet feature complete, in memory search engine.

## Highlights

* Inspired by commonly known Lucene design
* Fast in memory searching
* Low memory allocation for search execution
* Stemmers and stopwords handling for 30 languages
* Text, keyword, hierarchy and range (any comparable structure values) fields
* Synonym graph with multi word synonym support
* Fully configurable and extendable text analysis pipeline
* Searching with BM25 weighted document scoring
* Filtering on any field type by values or range
* Faceting without restricting facet value list by filtered values
* Result sorting by document score or range field values
* Fluent query builder

## Supported languages

* Internally

  Porter (English)

* via Snowball

  English, Arabic, Armenian, Basque, Catalan, Danish, Dutch, Finnish, French, German, Greek, Hindi,
  Hungarian, Indonesian, Irish, Italian, Lithuanian, Nepali, Norwegian, Portuguese, Romanian, Russian,
  Serbian, Spanish, Swedish, Tamil, Turkish, Yiddish

* via Morfologik

  Polish

## Getting started

> For more advanced usage check sample test or benchmark project contents.

Given sample product data set:

```json
// From https://dummyjson.com/products
[
  {
    "id": 1,
    "title": "iPhone 9",
    "description": "An apple mobile which is nothing like apple",
    "price": 549,
    "discountPercentage": 12.96,
    "rating": 4.69,
    "stock": 94,
    "brand": "Apple",
    "category": "smartphones"
  },
  ...
]

```

We define data model:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public double? DiscountPercentage { get; set; }
    public double? Rating { get; set; }
    public int? Stock { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
}
```

Then we define model to index mapper:

```csharp
public class ProductMapper : IIndexMapper<Product>
{
    public static TextField<Product> Text = new(ToIndexedText, new PorterAnalyzer());
    public static DecimalField<Product> Price = new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);
    public static DoubleField<Product> DiscountPercentage = new(x => x.DiscountPercentage, isFilterable: true, isFacetable: true, isSortable: true);
    public static DoubleField<Product> Rating = new(x => x.Rating, isFilterable: true, isFacetable: true, isSortable: true);
    public static DoubleField<Product> Stock = new(x => x.Stock, isFilterable: true, isFacetable: true, isSortable: true);
    public static KeywordField<Product> Brand = new(x => x.Brand, isFilterable: true, isFacetable: true);
    public static KeywordField<Product> Category = new(x => x.Category, isFilterable: true, isFacetable: true);

    public IEnumerable<Field> GetFields()
    {
        yield return Text;
        yield return Price;
        yield return DiscountPercentage;
        yield return Rating;
        yield return Stock;
        yield return Brand;
        yield return Category;
    }

    public string GetDocumentKey(Product item) => item.Id.ToString();

    public Product GetDocument(Product item) => item;

    private static string ToIndexedText(Product product)
    {
        var builder = new StringBuilder();

        builder.AppendLine(product.Title);
        builder.AppendLine(product.Description);
        builder.AppendLine(product.Brand);
        builder.AppendLine(product.Category);

        return builder.ToString();
    }
}
```

Now we can build index:

```csharp
public Index<Product> BuildIndex(IEnumerable<Product> items)
{
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

    return builder.Build();
}
```

Now we can run queries on built index:

```csharp
using var result = index.Query(
    index.QueryBuilder()
        .Search(ProductMapper.Text, allTextSynonym)
        .Filter(ProductMapper.Brand, Values.Any(topBrand))
        .Filter(ProductMapper.Price, from: 1, to: maxPrice - 1)
        .Facet(ProductMapper.Brand)
        .Facet(ProductMapper.Category)
        .Facet(ProductMapper.Price)
        .Sort(ProductMapper.Price, SortDirection.Descending));

foreach (var document in result.Documents)
{
    Console.WriteLine(document.Document.Title);
}
```

## Benchmarks

Query benchmarks and tests are performed using sample 100 product data set.

> Benchmark variants with suffix "100" use instead 100 times more product data. As observed due
> to internal structure pooling memory allocation per search execution is constant and independent
> from amount of indexed documents after initial allocation of pooled buffers.

```
BenchmarkDotNet v0.13.8, Windows 11 (10.0.22621.2283/22H2/2022Update/SunValley2)
12th Gen Intel Core i9-12900K, 1 CPU, 24 logical and 16 physical cores
.NET SDK 7.0.308
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
```

| Method                        | Mean       | Error     | StdDev    | Gen0   | Allocated |
|------------------------------ |-----------:|----------:|----------:|-------:|----------:|
| SearchFilterFacetSortQuery100 | 579.123 μs | 6.9726 μs | 6.1810 μs |      - |    1473 B |
| SearchFilterFacetSortQuery    |  12.778 μs | 0.2520 μs | 0.4347 μs | 0.0916 |    1472 B |
| SearchQuery                   |   7.449 μs | 0.1412 μs | 0.1681 μs | 0.0305 |     704 B |
| FilterQuery                   |   1.462 μs | 0.0244 μs | 0.0228 μs | 0.0458 |     720 B |
| FacetQuery                    |   9.634 μs | 0.1914 μs | 0.1880 μs | 0.0305 |     536 B |
| SortQuery                     |   3.453 μs | 0.0391 μs | 0.0347 μs | 0.0229 |     408 B |
| Query                         |   1.503 μs | 0.0300 μs | 0.0492 μs | 0.0191 |     312 B |

## License

Released under the MIT License
