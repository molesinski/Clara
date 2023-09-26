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
* Text, keyword, hierarchy and range fields
* Synonym tree with multi word synonym support
* Fully configurable and extendable text analysis pipeline
* Document scring with BM25 weight
* Filtering on any field type by values or range
* Faceting without restricting facet value list by filtered values
* Result sorting by document score or range field
* Fluent query builder

## Supported languages

* Internally

  Porter (English)

* Using [Clara.Analysis.Snowball](https://www.nuget.org/packages/Clara.Analysis.Snowball) package

  English, Arabic, Armenian, Basque, Catalan, Danish, Dutch, Finnish, French, German, Greek, Hindi,
  Hungarian, Indonesian, Irish, Italian, Lithuanian, Nepali, Norwegian, Portuguese, Romanian, Russian,
  Serbian, Spanish, Swedish, Tamil, Turkish, Yiddish

* Using [Clara.Analysis.Morfologik](https://www.nuget.org/packages/Clara.Analysis.Morfologik) package

  Polish

## Getting started

Given sample product data set from [dummyjson.com](https://dummyjson.com/products).

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
        yield return product.Id.ToString(CultureInfo.InvariantCulture);
        yield return product.Title;
        yield return product.Description;
        yield return product.Brand;
        yield return product.Category;
    }
}
```

Then we build our index.

```csharp
// When rebuilding index, this reference should be reused
var tokenEncoderStore = new SharedTokenEncoderStore();

var index = IndexBuilder.Build(Product.Items, new ProductMapper(), tokenEncoderStore);
```

With index built, can run queries against it. Result documents can be accessed with `Documents` property and
facet results via `Facets`. Documents are not paged, since search engine builds whole result set each time,
in order to perform facet values computation, while using pooled buffers for result construction. If paging
is needed, it can be added by simple `Skip`/`Take` logic on top `Documents` collection.

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
    Console.WriteLine($"  [{document.Document.Title}] => {document.Score}");
}

Console.WriteLine("Brands:");

foreach (var value in result.Facets.Field(ProductMapper.Brand).Values.Take(5))
{
    Console.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
}

Console.WriteLine("Categories:");

foreach (var value in result.Facets.Field(ProductMapper.Category).Values.Take(5))
{
    Console.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
}

var priceFacet = result.Facets.Field(ProductMapper.Price);

Console.WriteLine("Price:");
Console.WriteLine($"  [Min] => {priceFacet.Min}");
Console.WriteLine($"  [Max] => {priceFacet.Max}");
```

Running this query against sample data results in following output.

```
Documents:
  [Samsung Universe 9] => 3,3160777
  [iPhone X] => 2,9904046
  [iPhone 9] => 3,5479112
Brands:
  (x) [Apple] => 2
  (x) [Samsung] => 1
  ( ) [Huawei] => 1
Categries:
  ( ) [smartphones] => 3
Price:
  [Min] => 549
  [Max] => 1249
```

## Advanced scenarios

### Synonym maps

TODO

### Custom analyzers

Above code uses `PorterAnalyzer` which provides basic English language stemming. For other languages
[Clara.Analysis.Snowball](https://www.nuget.org/packages/Clara.Analysis.Snowball) or
[Clara.Analysis.Morfologik](https://www.nuget.org/packages/Clara.Analysis.Morfologik) packages can
be used. Those packages provide stem and stop token filters for all supported languages.

Below is shown example analyzer definition `PolishAnalyzer` using Morfologik.

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

It can be used for index mapper field definition as follows.

```csharp
public static TextField<Product> TextPolish = new(x => GetTextPolish(x), PolishAnalyzer);
```

### Custom range fields

Range fields represent index fields for `struct` values with `IComparable<T>` interface implementation.
Internally `DateTime`, `Decimal`, `Double` and `Int32` types are supported. Implementors can support any
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

### Custom `IAnalyzer`, `ITokenizer` and `IFilterToken`

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

| Method             | Mean        | Error       | StdDev      | Median      | Gen0      | Gen1      | Gen2      | Allocated   |
|------------------- |------------:|------------:|------------:|------------:|----------:|----------:|----------:|------------:|
| IndexX100          | 68,546.8 μs | 1,531.45 μs | 4,515.52 μs | 66,796.3 μs | 2428.5714 | 2285.7143 | 1000.0000 | 31207.57 KB |
| IndexSynonym       |    532.1 μs |     5.49 μs |     4.29 μs |    532.2 μs |   36.1328 |   12.6953 |         - |   559.79 KB |
| Index              |    468.1 μs |     6.06 μs |     5.67 μs |    467.6 μs |   34.6680 |   13.1836 |         - |   538.11 KB |
| SharedIndexX100    | 64,371.0 μs | 1,283.19 μs | 3,242.78 μs | 63,775.1 μs | 2250.0000 | 2000.0000 |  875.0000 | 29917.87 KB |
| SharedIndexSynonym |    500.6 μs |     7.09 μs |     6.63 μs |    502.4 μs |   32.2266 |   12.6953 |         - |   507.09 KB |
| SharedIndex        |    439.1 μs |     3.54 μs |     3.14 μs |    439.0 μs |   31.2500 |   10.7422 |         - |   485.41 KB |

### Query benchmarks

| Method           | Mean       | Error      | StdDev     | Gen0   | Allocated |
|----------------- |-----------:|-----------:|-----------:|-------:|----------:|
| ComplexQueryX100 | 584.200 μs | 11.3469 μs | 16.6321 μs |      - |    1593 B |
| ComplexQuery     |  12.117 μs |  0.0803 μs |  0.0751 μs | 0.0916 |    1592 B |
| SearchQuery      |   7.371 μs |  0.0566 μs |  0.0502 μs | 0.0381 |     712 B |
| FilterQuery      |   1.333 μs |  0.0139 μs |  0.0130 μs | 0.0458 |     744 B |
| FacetQuery       |   8.804 μs |  0.0496 μs |  0.0464 μs | 0.0305 |     600 B |
| SortQuery        |   3.396 μs |  0.0267 μs |  0.0250 μs | 0.0229 |     408 B |
| BasicQuery       |   1.393 μs |  0.0189 μs |  0.0168 μs | 0.0191 |     312 B |

> Due to internal buffer structures pooling, memory allocation per search execution is constant
> after initial allocation of pooled buffers.

## License

Released under the MIT License
