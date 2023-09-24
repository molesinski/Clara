# Clara

[![CI](https://dev.azure.com/molesinski/Clara/_apis/build/status/Clara?branchName=master)](https://dev.azure.com/molesinski/Clara/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/dt/clara.svg)](https://www.nuget.org/packages/clara) 
[![NuGet](https://img.shields.io/nuget/v/clara.svg)](https://www.nuget.org/packages/clara)
[![License](https://img.shields.io/github/license/molesinski/clara.svg)](https://github.com/molesinski/clara/blob/master/LICENSE)

Simple, yet feature complete, in memory search engine.

## Highlights

This library is meant for relatively small document sets (up to tenths of thousands) while maintaining
fast query times (around 1 milisecond). Updating index requires reindexing, which means building new index,
replacing in memory reference and discarding old one.

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

Given sample product data set from https://dummyjson.com/products.

```json
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
  }
]

```

We define data model.

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

Then we define model to index mapper. Mapper is a definition of how our index will be built from source
documents and what capabilities will it provide afterwards.

We only support single field searching, all text that is to be indexed has to be combined into
single field. We can provide more text fields, for example when we want to provide multiple language
support from single index. In such case we would combine text for each language and use adequate
analyzer.

For simple fields we define delegates that provide raw values for indexing. Each field can provide none,
one or more values, null values are automatically skipped during indexing. All simple fields can be marked
as filterable or facetable, while only range fields can be made sortable.

Built indexes have no persistence and reside only in memory. If index needs updating, it should be rebuild
and old one should be discarded. This is why fields have no names and can be referenced only by their
usually static definition.

`IIndexMapper<TSource>` interface is straightforward. It provides all fields collection, method to access document
key and method to access indexed document value. Indexed document value, which is provided in query results
can be different than index source document. To indicate such distinction use `IIndexMapper<TSouce, TDocument>`
type instead and return proper document type in `GetDocument` method implementation.

```csharp
public sealed class ProductMapper : IIndexMapper<Product>
{
    public static TextField<Product> Text { get; } = new(x => GetText(x), new PorterAnalyzer());
    public static DecimalField<Product> Price { get; } = new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);
    public static DoubleField<Product> DiscountPercentage { get; } = new(x => x.DiscountPercentage, isFilterable: true, isFacetable: true, isSortable: true);
    public static DoubleField<Product> Rating { get; } = new(x => x.Rating, isFilterable: true, isFacetable: true, isSortable: true);
    public static Int32Field<Product> Stock { get; } = new(x => x.Stock, isFilterable: true, isFacetable: true, isSortable: true);
    public static KeywordField<Product> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);
    public static KeywordField<Product> Category { get; } = new(x => x.Category, isFilterable: true, isFacetable: true);

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

    private static string GetText(Product product)
    {
        var builder = new StringBuilder();

        builder.AppendLine(product.Id.ToString(CultureInfo.InvariantCulture));
        builder.AppendLine(product.Title);
        builder.AppendLine(product.Description);
        builder.AppendLine(product.Brand);
        builder.AppendLine(product.Category);
        builder.AppendLine(CommonTextPhrase);

        return builder.ToString();
    }
}
```

Then we build our index.

```csharp
var builder =
    new IndexBuilder<Product, Product>(
        new ProductMapper());

foreach (var item in Product.Items)
{
    builder.Index(item);
}

var index = builder.Build();
```

With index built, can run queries on it. Result documents can be accessed with `Documents` property and
facet results via `Facets`. Documents are not paged, since engine has to build whole result set each time
for facet values computation, while using pooled buffers for result construction. If paging is needed,
it can be added by simple `Skip`/`Take` logic on top `Documents` collection.

```csharp
// Query result must always be disposed in order to return pooled buffers for reuse
using var result = index.Query(
    index.QueryBuilder()
        .Search(ProductMapper.Text, "smartphone")
        .Filter(ProductMapper.Brand, Values.Any("Apple", "Samsung"))
        .Filter(ProductMapper.Price, from: 300, to: 1500)
        .Facet(ProductMapper.Brand)
        .Facet(ProductMapper.Category)
        .Facet(ProductMapper.Price)
        .Sort(ProductMapper.Price, SortDirection.Descending));

Console.WriteLine("Documents:");

foreach (var document in result.Documents.Take(10))
{
    Console.WriteLine($"  [{document.Key}] => {document.Score} ({document.Document.Title})");
}

var brandFacet = result.Facets.Field(ProductMapper.Brand);

Console.WriteLine("Brands:");

foreach (var value in brandFacet.Values.Take(5))
{
    Console.WriteLine($"  [{value.Value}] => {value.Count} {(value.IsSelected ? "(x)" : "( )")}");
}

var priceFacet = result.Facets.Field(ProductMapper.Price);

Console.WriteLine("Price:");
Console.WriteLine($"  [Min] => {priceFacet.Min}");
Console.WriteLine($"  [Max] => {priceFacet.Max}");
```

Running this query against sample data results in following output.

```
Documents:
  [3] => 3,3160777 (Samsung Universe 9)
  [2] => 2,9904046 (iPhone X)
  [1] => 3,5479112 (iPhone 9)
Brands:
  [Apple] => 2 (x)
  [Samsung] => 1 (x)
  [Huawei] => 1 ( )
Price:
  [Min] => 549
  [Max] => 1249
```

## Advanced scenarios

### Custom analyzers

Above code uses `PorterAnalyzer` which provides basic English language stemming. For other languages
`Clara.Analysis.Snowball` or `Clara.Analysis.Morfologik` packages can be used. Those packages provide
stem and stop token filters for all supported languages.

For example you could define `PolishAnalyzer` like this.

```csharp
public static IAnalyzer PolishAnalyzer { get; } =
    new Analyzer(
        new BasicTokenizer(numberDecimalSeparator: ','), // Splits text into tokens
        new LowerInvariantTokenFilter(),                 // Transforms into lower case
        new CachingTokenFilter(),                        // Prevents new string instance creation
        new PolishStopTokenFilter(),                     // Language specific stop words default exclusion set
        new KeywordLengthTokenFilter(),                  // Exclude from stemming tokens with length 2 or less
        new KeywordDigitsTokenFilter(),                  // Exclude from stemming tokens containing digits
        new PolishStemTokenFilter());                    // Language specific token stemming
```

And then use it for index mapper field definition.

```csharp
public static TextField<Product> TextPolish = new(x => GetTextPolish(x), PolishAnalyzer);
```

### Custom range fields

Range fields represent index fields for `struct` values with `IComparable<T>` interface implementation.
By default `DateTime`, `Decimal`, `Double` and `Int32` types are supported. Implementors can support any
type that fullfills requirements by directly using `RangeField<T>` and providing `minValue` and `maxValue`
for a given type or by providing their own concrete implementation.

Below is example implementation for `DateOnly` structure type.

```csharp
public sealed class DateOnlyField<TSource> : RangeField<TSource, int>
{
    public DateOnlyField(Func<TSource, DateOnly?> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
        : base(
            valueMapper: valueMapper,
            minValue: DateOnly.MinValue,
            maxValue: DateOnly.MaxValue,
            isFilterable: isFilterable,
            isFacetable: isFacetable,
            isSortable: isSortable)
    {
    }

    public DateOnlyField(Func<TSource, IEnumerable<DateOnly>> valueMapper, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
        : base(
            valueMapper: valueMapper,
            minValue: DateOnly.MinValue,
            maxValue: DateOnly.MaxValue,
            isFilterable: isFilterable,
            isFacetable: isFacetable,
            isSortable: isSortable)
    {
    }
}
```

### Synonym maps

TODO

## Benchmarks

Index and query benchmarks and tests are performed using sample 100 product data set. Benchmark
variants with suffix "X100" use 100 times bigger index sizes.

```
BenchmarkDotNet v0.13.8, Windows 11 (10.0.22621.2283/22H2/2022Update/SunValley2)
12th Gen Intel Core i9-12900K, 1 CPU, 24 logical and 16 physical cores
.NET SDK 7.0.308
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
```

### Index benchmarks

| Method          | Mean        | Error       | StdDev      | Gen0      | Gen1      | Gen2      | Allocated   |
|---------------- |------------:|------------:|------------:|----------:|----------:|----------:|------------:|
| IndexX100       | 67,040.2 μs | 1,302.74 μs | 1,739.12 μs | 2500.0000 | 2375.0000 | 1125.0000 | 30734.56 KB |
| Index           |    468.7 μs |     7.40 μs |     6.56 μs |   34.1797 |   12.2070 |         - |   528.38 KB |
| SynonymMapIndex |    534.2 μs |     4.97 μs |     4.40 μs |   35.1563 |   11.7188 |         - |   552.81 KB |

### Query benchmarks

| Method                         | Mean       | Error     | StdDev    | Gen0   | Allocated |
|------------------------------- |-----------:|----------:|----------:|-------:|----------:|
| SearchFilterFacetSortQueryX100 | 570.530 μs | 4.3694 μs | 4.0871 μs |      - |    1585 B |
| SearchFilterFacetSortQuery     |  12.417 μs | 0.0557 μs | 0.0521 μs | 0.0916 |    1584 B |
| SearchQuery                    |   7.295 μs | 0.0281 μs | 0.0263 μs | 0.0381 |     704 B |
| FilterQuery                    |   1.410 μs | 0.0075 μs | 0.0066 μs | 0.0458 |     720 B |
| FacetQuery                     |   9.898 μs | 0.0679 μs | 0.0635 μs | 0.0305 |     648 B |
| SortQuery                      |   3.584 μs | 0.0164 μs | 0.0154 μs | 0.0229 |     408 B |
| Query                          |   1.439 μs | 0.0049 μs | 0.0046 μs | 0.0191 |     312 B |

> Due to internal buffer structures pooling, memory allocation per search execution is constant
> after initial allocation of pooled buffers.

## License

Released under the MIT License
