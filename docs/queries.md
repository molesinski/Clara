# Queries and Facets

## Building a query

The simplest form is `using var result = index.Query(q => q.Filter(...));`. The library manages the builder and `Query`, while the caller owns the result. You can also use `index.QueryBuilder()` or construct `new Query(index)` directly.

[`QueryBuilder`](../src/Clara/Querying/QueryBuilder.cs) allows one `Search`, one `Sort`, and one filter and facet per field instance. Repeating an operation throws an exception. `ToQuery()` transfers query ownership to the caller and consumes the builder. In contrast, `Query.AddFilter` and `AddFacet` replace an entry for the same field; when replacing a filter manually, dispose the previous object.

## Text search

- `SearchMode.Any` unions the document sets matching individual terms.
- `SearchMode.All` intersects the document sets for all terms.
- `null`, empty text, and whitespace-only text do not trigger a search or restrict results.
- Nonempty text that produces no terms after analysis, such as `...`, returns an empty result.
- An unknown term contributes no matches in `Any` and can empty the result in `All`.

A term can be a token or a group of alternative synonym phrases. A phrase requires all its tokens to occur in a document, but the index does not store their positions. This does not implement word adjacency checks, quoted phrase syntax, or fuzzy search. `PositionBoost` refers to positions in the analyzed query, not positions in the document.

## Filters and document keys

Filters on different fields are combined with AND. For keyword and hierarchy fields, `Any` means any selected value and `All` means every selected value. The filter value collection removes duplicates, trims text, and skips `null` and whitespace-only values. A filter that is empty after this cleanup is ignored.

Range filters include both bounds: `from <= value <= to`. A `null` bound leaves that side unbounded; two `null` bounds disable the filter. Reversed ranges or bounds outside the field's declared range throw exceptions. The public builder accepts `from` and `to`; the `RangeFilterExpression<TValue>` constructor uses `valueFrom` and `valueTo`.

`Include` and `Exclude` accept document keys, not objects or internal identifiers. They skip `null` values. An empty or all-null `Include` imposes no restriction. If at least one non-null value is supplied, only matching document keys from the current index are included; a selection containing no current keys yields an empty result. `Exclude` removes recognized documents. Keys do not undergo keyword label normalization.

`Include` checks both the encoder lookup and membership in the current index's document set. Labels, text tokens, unknown values, and keys retained from an older index are ignored unless they are also valid current document keys. Mixed selections retain valid keys and remove duplicates. [IncludeTests.cs](../src/Clara.Tests/IncludeTests.cs) covers these cases and composition with search, filters, facets, and exclusion.

## Execution order

[`Index.Query`](../src/Clara/Index.cs) starts with all documents, then:

1. Applies `Include`.
2. Executes the search and retains the scores.
3. Skips empty filters, orders the remaining filters, and intersects result sets.
4. Applies `Exclude`, including to sets retained for facets.
5. Computes facets from their respective document sets.
6. Sorts documents and transfers buffers to `QueryResult`.

The order in which filters are added to the builder is not the execution plan. `FilterExpressionComparer` places filters that do not require a separate retained facet set first, then uses the stores' `FilterOrder`. This is a simple heuristic based on structure sizes, not a selectivity estimate for specific values.

## When a facet excludes its own filter

| Field and filter | Its own filter affects the facet's document set |
| --- | --- |
| Keyword, `FilterMode.Any` | No |
| Keyword, `FilterMode.All` | Yes |
| Hierarchy, `Any` or `All` | Yes |
| Range | No |

Other filters, the search, and document restrictions still apply. For example, selecting brand Alpha through `Any` can restrict documents to Alpha while the brand facet still shows other brands available under the current price filter and search.

[`DocumentResultBuilder`](../src/Clara/Storage/DocumentResultBuilder.cs) implements this behavior: before the relevant filter, it retains a separate set, skips intersecting that set with the same field's filter, and applies subsequent filters on other fields.

### Facet result shapes

- Keyword facets return `Value`, `Count`, and `IsSelected`. Selected values come first, followed by descending count. A selected value with no matches remains present with a zero count.
- Hierarchy facets return selected nodes and their immediate children. Without a filter, navigation starts at `Root`. Children do not form a complete recursive tree; further navigation requires another query. Counts refer to documents, not path occurrences.
- Range facets return `MinValue` and `MaxValue`. When there are no values, both are `default(TValue)`; the API has no separate missing-data indicator. Account for this ambiguity when zero is a valid value.

Read results through `result.Facets.Field(mapper.Price)` or the relevant field. The requested facet must have been added to the query.

## Sorting and pagination

An explicit `Sort` supports one range field. Without it, search results are sorted by descending score; without a search, there is no ordering contract. Comparers do not break ties using document keys.

For multiple values, ascending order uses the minimum and descending order uses the maximum. A document with no value uses `field.MaxValue` or `field.MinValue`, respectively, for sorting; it may tie with a document containing that extreme value.

There is no internal top-k limit or pagination. `Skip` and `Take` operate on the complete result. If the application requires stable ordering for ties, establish that order explicitly before splitting results into pages. Scores remain available even when sorting by price.

Tests: [QueryTests.cs](../src/Clara.Tests/QueryTests.cs), [SynonymMapTests.cs](../src/Clara.Tests/SynonymMapTests.cs).
