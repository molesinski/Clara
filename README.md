﻿# Clara

[![CI](https://dev.azure.com/molesinski/Clara/_apis/build/status/Clara?branchName=master)](https://dev.azure.com/molesinski/Clara/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/dt/clara.svg)](https://www.nuget.org/packages/clara) 
[![NuGet](https://img.shields.io/nuget/v/clara.svg)](https://www.nuget.org/packages/clara)
[![License](https://img.shields.io/github/license/molesinski/clara.svg)](https://github.com/molesinski/clara/blob/master/LICENSE)

Simple, yet feature complete, in memory search engine.

## Highlights

This library is meant for relatively small document sets (up to tenths of thousands) while maintaining
fast query times (measured in low milliseconds). Index updates are not supported by design and full index
rebuild is required to reflect changes in source data.

Main features are:

* Inspired by well known Lucene design
* Fast in memory searching
* Low memory allocation for search execution
* Stemming and stop words handling for 31 languages
* Text, keyword, hierarchy and range fields
* Index time synonym maps with multi token support
* Cross field searching with BM25 scoring
* Filtering keyword and hierarchy fields by any or all values and range fields by value subrange
* Faceting without restricting facet values by field filters
* Result sorting by document scoring or range field values
* Fully configurable and extendable text analysis pipeline
* Fluent query builder

## Supported Languages

* Internally

  Porter (**English**)

* Using [Clara.Analysis.Snowball](https://www.nuget.org/packages/Clara.Analysis.Snowball) package

  Arabic, Armenian, Basque, Catalan, Danish, Dutch, **English**, Estonian, Finnish, French, German, Greek,
  Hindi, Hungarian, Indonesian, Irish, Italian, Lithuanian, Nepali, Norwegian, Portuguese, Romanian, Russian,
  Serbian, Spanish, Swedish, Tamil, Turkish, Yiddish

* Using [Clara.Analysis.Morfologik](https://www.nuget.org/packages/Clara.Analysis.Morfologik) package

  Polish

## A Quick Example

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

We define data model as follows.

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

Now we need to define index model mapper. Mapper is a definition of how our index will be built from source
documents and what capabilities will it provide afterwards.

Clara only supports single field searching, all text that is to be indexed has to be combined into
single field. We can provide more text fields, for example when we want to provide multiple language
support from single index. In such case we would combine text for each language and use adequate
analyzer.

For simple fields we define delegates that provide raw values for indexing. Each field can provide none,
one or more values, null values are automatically skipped during indexing. All non-text fields can be marked
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
    public TextField<Product> Text { get; } = new(GetText, new PorterAnalyzer());
    public DecimalField<Product> Price { get; } = new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);
    public DoubleField<Product> DiscountPercentage { get; } = new(x => x.DiscountPercentage, isFilterable: true, isFacetable: true, isSortable: true);
    public DoubleField<Product> Rating { get; } = new(x => x.Rating, isFilterable: true, isFacetable: true, isSortable: true);
    public Int32Field<Product> Stock { get; } = new(x => x.Stock, isFilterable: true, isFacetable: true, isSortable: true);
    public KeywordField<Product> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);
    public HierarchyField<Product> Category { get; } = new(x => x.Category, separator: "-", root: "all", HierarchyValueHandling.Path, isFilterable: true, isFacetable: true);

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

    private static IEnumerable<string?> GetText(Product product)
    {
        yield return product.Id.ToString();
        yield return product.Title;
        yield return product.Description;
        yield return product.Brand;
        yield return product.Category;
    }
}
```

Then we build our index.

```csharp
var mapper = new ProductMapper();
var index = IndexBuilder.Build(products, mapper);
```

With index built, we can run queries against it. Result documents can be accessed with `Documents` property and
facet results via `Facets`. Documents are not paged, since search engine builds whole result set each time,
in order to perform facet values computation, while using pooled buffers for result construction. If paging
is needed, it can be added by simple `Skip`/`Take` logic on top `Documents` collection.

```csharp
// Query result must always be disposed in order to return pooled buffers for reuse
using var result = index.Query(
    q => q
        .Search(mapper.Text, SearchMode.Any, "watch ring leather bag")
        .Filter(mapper.Brand, FilterMode.Any, "Eastern Watches", "Bracelet", "Copenhagen Luxe")
        .Filter(mapper.Category, FilterMode.Any, "womens")
        .Filter(mapper.Price, valueFrom: 10, valueTo: 90)
        .Facet(mapper.Brand)
        .Facet(mapper.Category)
        .Facet(mapper.Price)
        .Sort(mapper.Price, SortDirection.Descending));

Console.WriteLine("Documents:");

foreach (var document in result.Documents.Take(10))
{
    Console.WriteLine($"  [{document.Document.Title}] ${document.Document.Price} => {document.Score}");
}

Console.WriteLine("Brands:");

foreach (var value in result.Facets.Field(mapper.Brand).Values.Take(5))
{
    Console.WriteLine($"  {(value.IsSelected ? "(x)" : "( )")} [{value.Value}] => {value.Count}");
}

Console.WriteLine("Categories:");

foreach (var value in result.Facets.Field(mapper.Category).Values.Take(5))
{
    Console.WriteLine($"  (x) [{value.Value}] => {value.Count}");

    foreach (var child in value.Children)
    {
        Console.WriteLine($"    ( ) [{child.Value}] => {child.Count}");
    }
}

var priceFacet = result.Facets.Field(mapper.Price);

Console.WriteLine("Price:");
Console.WriteLine($"  [Min] => {priceFacet.Min}");
Console.WriteLine($"  [Max] => {priceFacet.Max}");
```

Running this query against sample data results in following output.

```
Documents:
  [Fashion Magnetic Wrist Watch] $60 => 3,31469
  [Leather Hand Bag] $57 => 6,3701906
  [Fancy hand clutch] $44 => 6,559135
  [Steel Analog Couple Watches] $35 => 3,4505985
  [Stainless Steel Women] $35 => 3,4802377
Brands:
  (x) [Eastern Watches] => 2
  (x) [Bracelet] => 2
  (x) [Copenhagen Luxe] => 1
  ( ) [LouisWill] => 2
  ( ) [Luxury Digital] => 1
Categories:
  (x) [womens] => 5
    ( ) [womens-watches] => 3
    ( ) [womens-bags] => 2
Price:
  [Min] => 35
  [Max] => 100
```

## Field Mapping

TODO

### Text Fields

TODO

### Keyword Fields

TODO

### Hierarchy Fields

TODO

### Range Fields

Range fields represent index fields for `struct` values with `IComparable<T>` interface implementation,
that is for comparable values, which can be within specific range. Range fields allow filtering by
subrange and their facet values contain matched documents minimum and maximum values. Sorting is dependant
on direction `Ascending` or `Descedning`. When sorting by `Ascending` order, then minumum document value
is used and documents without any value are treated as if they had `maxValue`. While when sorting by
`Descending` order maximum document value is used and documents without values have `minValue` assigned
to them.

Internally `DateTime`, `Decimal`, `Double` and `Int32` types are supported. Implementors can support
any type that fullfills requirements by directly using `RangeField<TValue>` and providing `minValue` and
`maxValue` for a given type. For example given `DateOnly` type, index field can be defined as follows.

```csharp
public static RangeField<DateOnly> DateOfBirth { get; } = new(x => x.DateOfBirth, minValue: DateOnly.MinValue, maxValue: DateOnly.MaxValue, isFilterable: true, isFacetable: true, isSortable: true);
```

Alternatively it is possible to provide own concrete implementation, by creating subclass of
`RangeField<TValue>`.

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

Defined `DateOnlyField` can be used now directly, without the need to specify `TValue` and `minValue`/`maxValue`
each time.

```csharp
public static DateOnlyField DateOfBirth { get; } = new(x => x.DateOfBirth, isFilterable: true, isFacetable: true, isSortable: true);
```

## Text Analysis

TODO

### Analyzers

TODO

Internally only `PorterAnalyzer` is provided for English language stemming. For other languages
[Clara.Analysis.Snowball](https://www.nuget.org/packages/Clara.Analysis.Snowball) or
[Clara.Analysis.Morfologik](https://www.nuget.org/packages/Clara.Analysis.Morfologik) packages can
be used. Those packages provide stem and stop token filters for all supported languages.

TODO

### Synonym Maps

TODO

### Extending Analysis Pipeline

TODO

## Benchmarks

Index and query benchmarks and tests are performed using sample 100 product data set. Benchmark
variants with `x100` suffix are based on data set multiplied 100 times.

```
BenchmarkDotNet v0.15.2, Windows 11 (10.0.26100.4351/24H2/2024Update/HudsonValley)
Intel Core i9-14900K 3.20GHz, 1 CPU, 32 logical and 24 physical cores
.NET SDK 9.0.301
  [Host]     : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2
```

### Tokenization Benchmarks

| Method             | Mean       | Error    | StdDev   | Allocated |
|------------------- |-----------:|---------:|---------:|----------:|
| StandardTokenizer  |   101.4 ns |  0.44 ns |  0.41 ns |         - |
| StandardAnalyzer   |   164.4 ns |  0.54 ns |  0.48 ns |         - |
| PorterAnalyzer     |   432.5 ns |  1.11 ns |  0.93 ns |         - |
| SynonymMap         |   524.2 ns |  2.28 ns |  2.02 ns |         - |
| EnglishAnalyzer    | 1,013.8 ns |  2.83 ns |  2.64 ns |         - |
| MorfologikAnalyzer | 3,680.1 ns | 12.93 ns | 10.10 ns |     944 B |

### Indexing Benchmarks

| Method             | Mean        | Error       | StdDev      | Allocated   |
|------------------- |------------:|------------:|------------:|------------:|
| IndexInstance_x100 | 52,143.3 μs | 1,032.90 μs | 1,608.10 μs | 25791.81 KB |
| IndexInstance      |    460.3 μs |     2.68 μs |     2.24 μs |   598.69 KB |
| IndexShared_x100   | 49,363.0 μs |   961.74 μs |   899.61 μs | 24510.22 KB |
| IndexShared        |    425.1 μs |     2.44 μs |     2.17 μs |   485.88 KB |

### Querying Benchmarks

| Method            | Mean         | Error       | StdDev      | Allocated |
|------------------ |-------------:|------------:|------------:|----------:|
| QueryComplex_x100 | 266,106.2 ns | 3,910.43 ns | 3,053.01 ns |     920 B |
| QueryComplex      |   6,425.7 ns |    39.15 ns |    36.62 ns |     920 B |
| QuerySearch       |   2,632.6 ns |    14.20 ns |    12.59 ns |     344 B |
| QueryFilter       |     665.8 ns |     3.40 ns |     3.01 ns |     448 B |
| QueryFacet        |   5,365.4 ns |    23.46 ns |    20.80 ns |     624 B |
| QuerySort         |   1,238.6 ns |     7.02 ns |     6.56 ns |     392 B |
| Query             |     412.3 ns |     1.88 ns |     1.76 ns |     296 B |

### Memory Allocations

Clara depends heavily on internal buffer pooling in order to provide minimal query execution memory
footprint. Due to that fact, memory allocation per search execution is constant after initial buffer
allocation. Although there are compromises being made regarding `Query` and `QueryResult` object
allocations to provide ease of use and proper disposal of internal buffers. 

## License

- Clara, [MIT License](LICENSE)
- [snowball](https://github.com/snowballstem/snowball), [BSD 3-Clause License](https://github.com/snowballstem/snowball/blob/master/COPYING)
- [Morfologik.Stemming](https://github.com/NightOwl888/Morfologik.Stemming), [Apache License 2.0](https://github.com/NightOwl888/Morfologik.Stemming/blob/main/LICENSE.txt)
