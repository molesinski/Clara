# Current Implementation Notes

The three confirmed defects found in revision `c3073b3` are corrected: document length accumulation, synonym search aggregation overrides, and `Include` key validation. The sections below describe the resulting contracts and regression coverage. The remaining exception-path concern is separate from these fixes.

## Corrected document lengths

[TextFieldStoreBuilder.cs](../src/Clara/Storage/TextFieldStoreBuilder.cs) now assigns the aggregation result once per emitted token:

```csharp
length = this.field.IndexScoreAggregation.Combine(length, value.Weight);
```

Length aggregates token weights across all values of a document's text field. `Sum` adds each weight once; `Max` keeps the largest emitted weight. Unit weights with `Sum` give lengths `1, 2, 3` for documents containing one, two, and three tokens. For `red red` at weight 2 plus `blue` at weight 3, the length is 7 under `Sum` and 3 under `Max`.

Synonym expansion contributes the emitted replacement tokens. Disabling expansion for a value uses its original analyzed tokens. Documents that emit no tokens are absent from the length map and therefore do not contribute to the document count or average length used by BM25. Custom aggregations fold the emitted weights in order, starting from zero.

Previously, adding the aggregation result to the previous length caused exponential growth under `Sum`. A regression with one matching token and 255 other tokens reproduced a non-finite BM25 score before the fix. [ScoringTests.cs](../src/Clara.Tests/ScoringTests.cs) now verifies weighted lengths for `Sum` and `Max`, expansion enabled and disabled, exact BM25 reference scores, and finite scores with the expected ordering for long documents.

The fix changes BM25 normalization and can change document rankings. Rebuild existing indexes to apply corrected lengths; an already built index retains its precomputed scores.

## Corrected aggregation overrides in synonym queries

[TextDocumentStore.cs](../src/Clara/Storage/TextDocumentStore.cs) uses the effective search aggregation throughout token and phrase processing. The query override takes precedence; when omitted, the field's `SearchScoreAggregation` is used. Alternatives within a `PhraseGroup` still select the maximum complete phrase score.

For `StandardAnalyzer`, `Similarity.TF`, document `red blue`, rule `alias => red blue`, unit weights, and default `Sum` index aggregation:

| Field aggregation | Query | Query aggregation | Score |
| --- | --- | --- | --- |
| `Sum` | `alias` | `Max` | 1 |
| `Max` | `alias` | `Sum` | 2 |
| `Sum` | `alias` | Omitted | 2 |
| `Max` | `alias` | Omitted | 1 |

Previously, phrase internals ignored the query override and used the field's aggregation. [ScoringTests.cs](../src/Clara.Tests/ScoringTests.cs) covers overrides in both directions, default fallback, `Any` and `All`, partial matches, multiple synonym alternatives, and composition with position boosts and other query terms. The override controls scoring; it does not relax the requirement that all tokens of a phrase must match.

## Corrected Include validation

The encoder assigns identifiers to document keys, text tokens, and labels, and a shared store can retain identifiers from previous indexes. [Index.cs](../src/Clara/Index.cs) now requires both a successful encoder lookup and membership in the current index's `allDocuments` set before including an identifier.

A known label such as `A` that is not a current document key now produces an empty result when it is the only requested key. Previously, this could create a phantom result entry and throw `KeyNotFoundException` during result enumeration. Historical keys from a shared encoder are ignored in the replacement index, while queries against the original index can still retrieve their documents.

An empty, null, or all-null selection remains unrestricted. Once a non-null value is supplied, only valid current keys are selected; an entirely invalid selection returns no documents. Mixed selections retain valid keys, discard invalid ones, and remove duplicates. A string that is both a token and a current document key remains a valid key.

[IncludeTests.cs](../src/Clara.Tests/IncludeTests.cs) covers labels, text tokens, unknown and blank values, historical keys with shared and instance stores, mixed selections, duplicate keys, empty selections, and composition with search, filters, facets, and exclusion.

## Remaining concern with leases on exception paths

**Conclusion from the code.** In [Index.cs](../src/Clara/Index.cs), some leases are protected by `using`, but `documentResultBuilder.Dispose()` runs at the end of the normal path. `documentScoring`, the result list, and facets transfer to a new owner only when `QueryResult` is created. Similarly, the destination `documentScores` in `TextDocumentStore.Search` is not protected against an exception before the result is returned.

An exception from the `positionBoost` callback or a later operation can bypass returning some objects to the pool. This does not establish a permanent managed-memory leak, but it risks losing buffer reuse and increasing allocations. When fixing it, identify when ownership transfers and use `try/finally` where an object has not yet been transferred. Check repeated failing calls as well as a single successful result.

Direct `Query.AddFilter` calls replace a previous filter without disposing it. `QueryBuilder` prevents duplicate filters, so this issue concerns manual use of the lower-level API.

## Other behavior relevant to integration

- A range facet with no values returns `default(TValue)` for both bounds, without a missing-data flag. An empty range cannot be directly distinguished from a valid `0..0` range.
- A probe with brands A and B confirmed that a keyword facet retains both brands with an `Any` filter on A, but shows only A with `All`. Filtering values 10, 20, and 30 to range `10..10` produced a facet maximum of 30.
- `SharedTokenEncoderStore` retains historical vocabulary. Retention follows from its design and needs measurement during long-running use with a changing vocabulary.
- Sort ties have no secondary ordering key. An application requiring stable pagination must define that order.

## Regression verification and further tests

The 35 new regression cases ran on .NET 10 before the fixes: 23 failed and 12 passed. With the fixes, all 212 tests pass on each of `net472`, `net8.0`, `net9.0`, and `net10.0`, for 848 successful executions. The Release solution build also passes with no warnings or errors.

Remaining test priorities include concurrent use, callback exceptions, broader facet semantics for each mode, and result ownership after `Dispose`. Add assertions for new contracts before changing the engine.
