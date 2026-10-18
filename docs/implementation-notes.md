# Current Implementation Notes

These observations apply to revision `c3073b3`. They distinguish results confirmed by small execution probes from conclusions based on code flow. The documentation does not change the implementation; it identifies areas relevant to future fixes and regression tests.

## Document length in BM25

**Confirmed by execution.** The `Index` method in [TextFieldStoreBuilder.cs](../src/Clara/Storage/TextFieldStoreBuilder.cs) uses:

```csharp
length += this.field.IndexScoreAggregation.Combine(length, value.Weight);
```

With `Sum`, this gives `L[n+1] = 2 * L[n] + weight`. Texts `one`, `one two`, and `one two three`, each in a separate document with `StandardAnalyzer` and weight 1, pass lengths `1, 3, 7` to `Similarity.Transform`. A plain sum of weights would instead be `1, 2, 3`.

This affects BM25 normalization: length grows exponentially, and sufficiently long text can cause `float` to reach infinity and subsequent calculations to produce `NaN`. The first three values were reproduced using a custom `Similarity` class that records the `documentLengths` argument; overflow is a conclusion from the recurrence, not a measurement made here.

Before fixing this, define length semantics for `Sum`, `Max`, and expanded synonyms. Add tests for lengths, finite scores, and ranking of short and long documents. Even replacing `+=` with assignment requires confirming that contract.

## Aggregation overrides in synonym queries

**Confirmed by execution.** [TextDocumentStore.cs](../src/Clara/Storage/TextDocumentStore.cs) passes the aggregation parameter to helper `Search` methods, but internal phrase token combination uses `this.searchScoreAggregation`, the field's setting.

Reproduction setup: `StandardAnalyzer`, `Similarity.TF`, document `red blue`, rule `alias => red blue`, unit weights, and default `Sum` index aggregation.

| Field aggregation | Query | Query aggregation | Score |
| --- | --- | --- | --- |
| `Sum` | `alias` | `Max` | 2 |
| `Sum` | `red blue` | `Max` | 1 |
| `Max` | `alias` | `Max` | 1 |

The override works between terms but not inside a replacement phrase. If it is intended to apply throughout the search, tests must cover mixed aggregations, multiple alternatives, and position boosts. Using `Max` for `PhraseGroup` alternatives is a separate, explicit implementation decision.

## Include can accept a token instead of a document key

**Confirmed by execution.** The encoder in [TokenEncoderBuilder.cs](../src/Clara/Storage/TokenEncoderBuilder.cs) assigns identifiers to document keys, tokens, and labels. The `Include` path in [Index.cs](../src/Clara/Index.cs) only checks `TryEncode`. The first intersection in `DocumentResultBuilder` copies the supplied identifiers without intersecting them with `allDocuments`.

Probe: an index contains keys `1`, `2`, and `3` and brand label `A`, but no document with key `A`. A query restricted only by `q.Include(new[] { "A" })` returns `Documents.Count == 1`, after which reading `Documents[0]` throws `KeyNotFoundException`.

The same risk follows from historical keys retained in a shared store. Membership in the current index must be checked before accepting an identifier. A regression test should cover a regular key, a label, a text token, a deleted document's key, and a value entirely unknown to the encoder. Until this is fixed, applications should restrict `Include` to keys from the current dataset.

## Returning leases on exception paths

**Conclusion from the code.** In [Index.cs](../src/Clara/Index.cs), some leases are protected by `using`, but `documentResultBuilder.Dispose()` runs at the end of the normal path. `documentScoring`, the result list, and facets transfer to a new owner only when `QueryResult` is created. Similarly, the destination `documentScores` in `TextDocumentStore.Search` is not protected against an exception before the result is returned.

An exception from the `positionBoost` callback or a later operation can bypass returning some objects to the pool. This does not establish a permanent managed-memory leak, but it risks losing buffer reuse and increasing allocations. When fixing it, identify when ownership transfers and use `try/finally` where an object has not yet been transferred. Check repeated failing calls as well as a single successful result.

Direct `Query.AddFilter` calls replace a previous filter without disposing it. `QueryBuilder` prevents duplicate filters, so this issue concerns manual use of the lower-level API.

## Behavior relevant to integration

- A range facet with no values returns `default(TValue)` for both bounds, without a missing-data flag. An empty range cannot be directly distinguished from a valid `0..0` range.
- An empty `Include` means no restriction. In a three-document probe, it returned three results; a collection containing one unknown key returned zero.
- A probe with brands A and B confirmed that a keyword facet retains both brands with an `Any` filter on A, but shows only A with `All`. Filtering values 10, 20, and 30 to range `10..10` produced a facet maximum of 30.
- `SharedTokenEncoderStore` retains historical vocabulary. Retention follows from its design and needs measurement during long-running use with a changing vocabulary.
- Sort ties have no secondary ordering key. An application requiring stable pagination must define that order.

## Priorities for further tests

Existing tests passed on all four targets, but they contain no numeric assertions for `Score`, dedicated `Include`/`Exclude` tests, or tests of concurrent use and callback exceptions. Prioritize `Include` correctness and the scoring regressions above, then facet semantics for each mode and result ownership after `Dispose`.

Treat the current 177 tests per target as confirmation of the scenarios they cover. Add assertions for new contracts before changing the engine.
