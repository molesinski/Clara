# Clara

[![CI](https://dev.azure.com/molesinski/Clara/_apis/build/status/Clara?branchName=master)](https://dev.azure.com/molesinski/Clara/_build/latest?definitionId=1&branchName=master)
[![NuGet](https://img.shields.io/nuget/dt/clara.svg)](https://www.nuget.org/packages/clara)
[![NuGet](https://img.shields.io/nuget/v/clara.svg)](https://www.nuget.org/packages/clara)
[![License](https://img.shields.io/github/license/molesinski/clara.svg)](https://github.com/molesinski/clara/blob/master/LICENSE)

Simple, yet feature complete, in memory search engine.

Developer documentation: [architecture, API, development, and implementation notes](docs/README.md).

## Highlights

This library is meant for relatively small document sets (up to tens of thousands) while maintaining
fast query times (measured in low milliseconds). Index updates are not supported by design and full index
rebuild is required to reflect changes in source data.

Main features are:

* Inspired by well known Lucene design
* Fast in memory searching
* Low memory allocation for search execution
* Multilingual stemming and configurable stopword lists
* Text, keyword, hierarchy and range fields
* Index time synonym maps with multi token support
* Searching across document properties combined into one text field, with BM25 scoring
* Filtering keyword and hierarchy fields by any or all values and range fields by value subrange
* Faceting that can retain alternatives excluded by the facet's own field filter
* Result sorting by document scoring or range field values
* Fully configurable and extendable text analysis pipeline
* Fluent query builder

## Installation

Add the core package to your application:

```shell
dotnet add package Clara
```

For additional language analyzers, add `Clara.Analysis.Snowball` or `Clara.Analysis.Morfologik`.
The projects target .NET Standard 2.0/2.1 and .NET 8/9/10. The examples below use modern C#;
the `DateOnly` example requires .NET 6 or later. Use these namespaces for mapping and querying:

```csharp
using Clara;
using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
```

## Supported Languages

* Internally

  Porter (**English**)

* Using [Clara.Analysis.Snowball](https://www.nuget.org/packages/Clara.Analysis.Snowball) package

  Arabic, Armenian, Basque, Catalan, Danish, Dutch, **English**, Estonian, Finnish, French, German, Greek,
  Hindi, Hungarian, Indonesian, Irish, Italian, Lithuanian, Nepali, Norwegian, Polish, Portuguese, Romanian, Russian,
  Serbian, Spanish, Swedish, Tamil, Turkish, Yiddish

* Using [Clara.Analysis.Morfologik](https://www.nuget.org/packages/Clara.Analysis.Morfologik) package

  Polish

## A Quick Example

The repository includes a sample product dataset originally obtained from
[dummyjson.com](https://dummyjson.com/products), saved in [Product.json](src/Clara.Tests/Product.json).
The following is one record; load the full fixture into `products` to run the catalog query below.

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
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public double? DiscountPercentage { get; set; }
    public double? Rating { get; set; }
    public int? Stock { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
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

Built indexes have no persistence and reside only in memory. Rebuild an index when its source data changes.
Fields have no names: keep their definitions in mapper properties and use those same instances when querying.

`IIndexMapper<TSource>` interface is straightforward. It provides all fields collection, method to access document
key and method to access indexed document value. Indexed document value, which is provided in query results
can be different than index source document. To indicate such distinction use `IIndexMapper<TSource, TDocument>`
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
        .Filter(mapper.Price, from: 10, to: 90)
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
Console.WriteLine($"  [Min] => {priceFacet.MinValue}");
Console.WriteLine($"  [Max] => {priceFacet.MaxValue}");
```

The query returns matching products sorted by descending price, plus brand, category, and price facets.
The following output uses the bundled fixture and invariant number formatting; scores depend on the dataset,
analyzer, and scoring settings.

```
Documents:
  [Fashion Magnetic Wrist Watch] $60 => 3.316201
  [Leather Hand Bag] $57 => 6.3456607
  [Fancy hand clutch] $44 => 6.5686703
  [Steel Analog Couple Watches] $35 => 3.4590023
  [Stainless Steel Women] $35 => 3.4776275
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

Implement `IIndexMapper<TSource>` to provide document keys, returned documents, and field definitions.
Use `IIndexMapper<TSource, TDocument>` when results should contain a different projection. Keys must be unique
within the index. `GetDocument` does not copy the object, so changing it later does not update indexed values.

Each field reads one value or an enumerable of values from a source document. Null values are skipped.
Non-text fields require at least one capability flag; enable only the operations your queries need.

| Field | Value type | Supported operations |
| --- | --- | --- |
| `TextField<TSource>` | Strings or `TextWeight` values | Search and scoring |
| `KeywordField<TSource>` | Labels | Filtering and faceting |
| `HierarchyField<TSource>` | Hierarchical identifiers or paths | Filtering and hierarchical faceting |
| `RangeField<TSource, TValue>` | Comparable value types | Filtering, min/max facets, and sorting |

Include every field in `GetFields()` and query with the same field instances used to build the index.
See [indexing and field storage](docs/indexing.md) for the build lifecycle and encoder options.

### Text Fields

`TextField<TSource>` requires an analyzer and accepts a string, multiple strings, or weighted text values.
Collect properties such as title and description into one field to search them together. A query searches
one text field; separate fields can provide different languages or analysis rules.

Use `TextWeight` to give selected source values more influence on scoring:

```csharp
var weightedText = new TextField<Product>(
    product => new[]
    {
        new TextWeight(product.Title, weight: 3f, expandSynonyms: true),
        new TextWeight(product.Description, weight: 1f, expandSynonyms: true),
    },
    new PorterAnalyzer(),
    similarity: Similarity.BM25());
```

Plain strings use weight 1 and enable synonym expansion. Empty text is ignored. By default, BM25 scores
the indexed terms, and `ScoreAggregation.Sum` combines weights during indexing and scores during search.
`ScoreAggregation.Max`, `Similarity.TF`, and `Similarity.TFIDF` are also available. A larger input weight
does not imply a proportional final BM25 score. See [scoring](docs/scoring.md) for the calculation stages.

`SearchMode.Any` accepts documents matching any query term; `SearchMode.All` requires every term.
Search uses analyzed tokens, without fuzzy matching or quoted phrase adjacency checks.

### Keyword Fields

Keyword fields store whole labels without text analysis. Labels are trimmed, empty values are skipped,
and comparisons are case-sensitive: `Apple` and `apple` are distinct. Repeated values in a document
count only once in a facet.

```csharp
var brand = new KeywordField<Product>(
    product => product.Brand,
    isFilterable: true,
    isFacetable: true);
```

For multivalued labels, return an `IEnumerable<string?>` from the mapper delegate. `FilterMode.Any`
matches any selected label; `FilterMode.All` requires all selected labels in the document. Filters on
different fields are combined with AND. A keyword facet returns each value's `Count` and `IsSelected`.
With an `Any` filter on that field, its facet retains alternatives while respecting the other filters;
an `All` filter also restricts its own facet.

### Hierarchy Fields

Hierarchy fields index a node and its ancestors. Set a separator, a synthetic root, and how node values
are identified. For a category such as `womens-watches`:

```csharp
var category = new HierarchyField<Product>(
    product => product.Category,
    separator: "-",
    root: "all",
    valueHandling: HierarchyValueHandling.Path,
    isFilterable: true,
    isFacetable: true);
```

`Path` indexes `all`, `womens`, and `womens-watches`. `Identifiers` instead uses `all`, `womens`, and
`watches`; use it only when node identifiers are unambiguous across branches. Do not include the synthetic
root in source paths. A mapper may return multiple paths per document.

Filtering `womens` includes its descendants. Hierarchy facets expose selected nodes and their immediate
`Children`; with no selection, navigation starts at the root. Fetch another facet result to navigate deeper.
Counts represent documents, not the number of paths. See [query and facet semantics](docs/queries.md).

### Range Fields

Range fields accept a `struct` implementing `IComparable<TValue>`. A mapper returns a nullable value or an
enumerable of values; values outside the declared `minValue`/`maxValue` bounds are skipped during indexing.
Range filters use inclusive `from` and `to` bounds, with `null` leaving a side unbounded. For multiple values,
a document matches when any value is in the requested range.

Range facets expose `MinValue` and `MaxValue`, excluding their own field filter while respecting other
constraints. With no values, both bounds are `default(TValue)`. Ascending sorting uses a document's minimum
value; descending sorting uses its maximum. Missing values use the field's upper or lower bound respectively,
so they sort last or tie with documents at that bound. Sort ties have no guaranteed secondary ordering.

Internally `DateTime`, `Decimal`, `Double` and `Int32` types are supported. Implementors can support
any type that fulfills these requirements by using `RangeField<TSource, TValue>` and providing `minValue` and
`maxValue` for a given type. For example given `DateOnly` type, index field can be defined as follows.

```csharp
public sealed class Person
{
    public DateOnly? DateOfBirth { get; set; }
}
```

```csharp
var dateOfBirth = new RangeField<Person, DateOnly>(
    person => person.DateOfBirth,
    minValue: DateOnly.MinValue,
    maxValue: DateOnly.MaxValue,
    isFilterable: true,
    isFacetable: true,
    isSortable: true);
```

Alternatively it is possible to provide own concrete implementation, by creating subclass of
`RangeField<TSource, TValue>`.

```csharp
public sealed class DateOnlyField<TSource> : RangeField<TSource, DateOnly>
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
var dateOfBirth = new DateOnlyField<Person>(
    person => person.DateOfBirth,
    isFilterable: true,
    isFacetable: true,
    isSortable: true);
```

## Text Analysis

Text analysis turns source text and query text into matching terms. A tokenizer finds tokens; an ordered
filter pipeline normalizes them, removes stopwords, protects selected keywords, or reduces words to stems.
The text field uses its configured analyzer on both sides. Rebuild the index after changing analysis rules.

`StandardTokenizer` recognizes letters, digits, and underscores, with rules for apostrophes and numeric
punctuation. `WhitespaceTokenizer` splits on whitespace only. Both skip tokens longer than
`Token.MaximumLength` (255 UTF-16 code units). Token positions are token indices, not character offsets.

### Analyzers

The core library provides `StandardAnalyzer` for lowercase normalization and optional stopwords, and
`PorterAnalyzer` for English stemming, possessive handling, and a default English stopword list.

For additional language analyzers,
[Clara.Analysis.Snowball](https://www.nuget.org/packages/Clara.Analysis.Snowball) or
[Clara.Analysis.Morfologik](https://www.nuget.org/packages/Clara.Analysis.Morfologik) packages can
be used. Snowball provides analyzers such as `EnglishAnalyzer` and `PolishAnalyzer`; Morfologik provides
the dictionary-based `MorfologikAnalyzer` for Polish. These classes also use the `Clara.Analysis` namespace.

```csharp
var analyzer = new PorterAnalyzer(
    stopwords: Array.Empty<string>(),
    keywords: new[] { "running" });

var source = analyzer.CreateTokenTermSource();
var terms = source.GetTerms("RUNNING shoes")
    .Select(term => term.Token.ToString())
    .ToArray();
// terms: ["running", "shoe"]
```

For language analyzers, `stopwords: null` selects their built-in list; an empty collection disables removal.
`StandardAnalyzer` has no default stopwords. Protected keywords should match the normalized token form
at that pipeline stage, typically lowercase.

A token source reuses its character buffer. Convert tokens to strings during enumeration if you need to
retain them, and use separate sources for concurrent or nested enumerations. Morfologik emits one selected
lemma rather than performing contextual disambiguation; see [text analysis](docs/text-analysis.md) for details.

### Synonym Maps

Synonym maps normalize related words and multiword expressions with the field's analyzer. Construct a
`SynonymMap` from rules and pass the **same analyzer instance** to both the map and the text field:

```csharp
using Clara.Analysis.Synonyms;

var analyzer = new StandardAnalyzer();
var rules = Synonym.Parse("mug, cup\ni phone, i-phone => iphone");
var synonyms = new SynonymMap(analyzer, rules);
var text = new TextField<Product>(product => product.Title, analyzer, synonyms);
```

Comma-separated phrases define an equivalence group. A rule with `=>` maps the left side to the right;
the original source is retained only if it is also a target. Rules can also be built with
`EquivalencySynonym` and `ExplicitMappingSynonym`. Equivalence groups expand by default; passing `expand: false`
to `SynonymMap` uses their first analyzed phrase as the target. The parser accepts blank lines and `#` comment lines;
invalid lines can be skipped, so validate externally supplied rules.

During indexing, replacement tokens are emitted unless a `TextWeight` is constructed with `expandSynonyms: false`.
During querying, all tokens of a synonym phrase must match, while alternative phrases are combined using
the maximum score. This does not enforce adjacency in the document. Disabling expansion for an indexed
value does not disable query-side synonyms. See [synonym processing](docs/text-analysis.md#synonyms).

### Extending Analysis Pipeline

Compose `Analyzer` with an `ITokenizer` and filters in execution order. A custom `ITokenFilter` can
continue with `next(token)`, discard a token by returning `default`, or return a token directly to stop
the chain. For example, this filter removes tokens shorter than three characters:

```csharp
public sealed class ShortTokenFilter : ITokenFilter
{
    public Token Process(Token token, TokenFilterDelegate next)
    {
        if (next is null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        return token.Length < 3 ? default : next(token);
    }
}
```

```csharp
var analyzer = new Analyzer(
    new StandardTokenizer(),
    new LowerInvariantTokenFilter(),
    new ShortTokenFilter(),
    new PorterStemTokenFilter());
```

The built-in `LengthStopTokenFilter(minimumLength: 3)` provides the same length check. Place stopword and
keyword filters after the normalization they rely on, and stemming last: Porter, Snowball, and Morfologik
stem filters terminate the chain. Tokens backed by strings are read-only; tokens backed by tokenizer
buffers can be modified in place. Keep filters stateless or provide synchronization for shared state.
Implement `ITokenizer` or `IAnalyzer` when an entirely different token source is needed.

## Benchmarks

The tables below are historical measurements from the environment shown. They predate the recent scoring,
Snowball, and pool ownership changes. See [current targeted comparisons](docs/development-and-testing.md#benchmarks)
for recorded update measurements and commands to benchmark your own workload.

Index and query benchmarks and tests are performed using sample 100 product data set. Benchmark
variants with `x100` suffix are based on data set multiplied 100 times.

```
BenchmarkDotNet v0.15.6, Windows 11 (10.0.26200.7171)
Intel Core i9-14900K 3.20GHz, 1 CPU, 32 logical and 24 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
```

### Tokenization Benchmarks

| Method               | Mean       | Error    | StdDev   | Allocated |
|--------------------- |-----------:|---------:|---------:|----------:|
| StandardTokenizer    |   102.6 ns |  1.07 ns |  0.95 ns |         - |
| StandardAnalyzer     |   162.7 ns |  0.97 ns |  0.91 ns |         - |
| PorterAnalyzer       |   362.7 ns |  4.02 ns |  3.76 ns |         - |
| SynonymMap           |   453.3 ns |  2.89 ns |  2.70 ns |         - |
| EnglishAnalyzer      |   908.1 ns |  2.48 ns |  2.32 ns |         - |
| PolishAnalyzer       |   477.5 ns |  6.79 ns |  6.35 ns |         - |
| MorfologikAnalyzer   | 3,319.5 ns | 11.43 ns | 10.69 ns |     944 B |
| LucenePolishAnalyzer | 1,594.3 ns | 31.20 ns | 47.65 ns |    1808 B |

### Indexing Benchmarks

| Method             | Mean        | Error     | StdDev      | Allocated   |
|------------------- |------------:|----------:|------------:|------------:|
| IndexInstance_x100 | 47,068.7 μs | 739.07 μs |   577.02 μs | 25791.01 KB |
| IndexInstance      |    434.3 μs |   4.19 μs |     3.92 μs |   598.65 KB |
| IndexShared_x100   | 45,486.0 μs | 899.20 μs | 1,104.29 μs | 24509.18 KB |
| IndexShared        |    406.5 μs |   2.25 μs |     2.10 μs |   485.86 KB |

### Querying Benchmarks

| Method            | Mean         | Error       | StdDev       | Allocated |
|------------------ |-------------:|------------:|-------------:|----------:|
| QueryComplex_x100 | 248,025.7 ns | 4,936.41 ns | 10,412.57 ns |     856 B |
| QueryComplex      |   6,046.9 ns |    94.14 ns |     88.06 ns |     856 B |
| QuerySearch       |   2,311.9 ns |    33.06 ns |     30.92 ns |     344 B |
| QueryFilter       |     577.7 ns |     3.96 ns |      3.70 ns |     448 B |
| QueryFacet        |   5,170.8 ns |    21.16 ns |     16.52 ns |     624 B |
| QuerySort         |   1,181.3 ns |    10.64 ns |      9.95 ns |     328 B |
| Query             |     383.8 ns |     1.54 ns |      1.44 ns |     296 B |

### Memory Allocations

Clara reuses buffers to reduce allocations on repeated queries. After warming the pools and their buffer
capacities for a workload, reuse avoids allocating those buffers again. Larger result sets, pool misses,
and concurrent queries can still cause allocations. `Query`, `QueryResult`, and facet wrappers also allocate.

Always dispose query results and materialize any data that must outlive them. Pool leases use shared,
versioned state to prevent repeated returns through copied leases, but accessing a buffer after its lease
is disposed remains invalid. See [memory and concurrency](docs/memory-and-concurrency.md) for ownership rules.

## License

- Clara, [MIT License](LICENSE)
- [snowball](https://github.com/snowballstem/snowball), [BSD 3-Clause License](https://github.com/snowballstem/snowball/blob/master/COPYING)
- [Morfologik.Stemming](https://github.com/NightOwl888/Morfologik.Stemming), [Apache License 2.0](https://github.com/NightOwl888/Morfologik.Stemming/blob/main/LICENSE.txt)
