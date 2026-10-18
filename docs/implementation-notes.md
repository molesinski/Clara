# Current Implementation Notes

The three confirmed defects found in revision `c3073b3` are corrected: document length accumulation, synonym search aggregation overrides, and `Include` key validation. The subsequently identified pool ownership and concurrent return problems are also corrected. The sections below describe the resulting contracts and regression coverage.

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

## Corrected pool ownership on exception paths

[Index.cs](../src/Clara/Index.cs) disposes the mutable document result builder in `finally`. Until a `QueryResult` is constructed successfully, the method owns the scoring dictionary, document list, and facet results. Its exception handler disposes these outputs, including each completed facet's nested buffers before returning the facet list. On success, ownership transfers to `QueryResult`; callers must still dispose the result.

[TextDocumentStore.Search](../src/Clara/Storage/TextDocumentStore.cs) returns its destination score dictionary if term enumeration, position boosting, or aggregation throws before ownership transfers to `DocumentScoring`. Existing `using` scopes return temporary term and phrase buffers. Filter value constructors also return their set if enumeration fails. Cleanup rethrows the original exception.

Previously, these exceptions could lose pooled buffers, increasing allocations on subsequent queries. This was a loss of reuse, not a permanent managed-memory leak. [PoolOwnershipTests.cs](../src/Clara.Tests/PoolOwnershipTests.cs) snapshots pooled object identities and verifies their return after repeated failures in boosts, aggregation, selections, filter construction, and later facet/sort stages. It also checks that a live successful result retains its buffers and that repeated result disposal does not return buffers twice. These tests run without parallel access to the shared pools.

Direct `Query.AddFilter` replacement now disposes the previous filter for that field. Re-adding the same instance keeps it alive. Once added, a filter belongs to the query; do not share it between queries. `QueryBuilder` continues to reject duplicate filters.

## Corrected concurrent pool returns

[ObjectPoolSlim.Return](../src/Clara/Utils/ObjectPoolSlim.cs) uses `Interlocked.CompareExchange` to publish a reset instance only into an empty slot. If another return claims that slot first, the loop tries the next slot. The previous check followed by plain assignment could overwrite another returned object and lose its reuse even when capacity remained.

[ObjectPoolSlimTests.cs](../src/Clara.Tests/ObjectPoolSlimTests.cs) synchronizes four returners for 1,000 rounds, verifies that all four original instances remain reusable without replacement allocations, and checks reset behavior. Full pools may still discard returned instances.

## Copied lease disposal

The shared-state/version approach from Implicit commit `f2ea517` is also applied to [ObjectPoolSlimLease.cs](../src/Clara/Utils/ObjectPoolSlimLease.cs). Copies of the readonly struct share metadata, while each subsequent lease receives a new version. Only the first matching atomic return claim resets and publishes the instance. This prevents duplicate entries in the pool and stale copies resetting an instance already borrowed by another caller. A default lease can be disposed safely; a failing resetter closes the lease and discards the instance without retrying on later disposal.

Clara retains all constructor overloads, `sizeFactor`, and default `IResettable` support. The extra metadata object is created only with a new pooled instance, then reused. Access through `Instance` after disposal remains unsupported; the getter does not validate ownership.

[ObjectPoolSlimLeaseTests.cs](../src/Clara.Tests/ObjectPoolSlimLeaseTests.cs) covers repeated, copied, boxed, and concurrent disposal; stale copies across later leases; default leases; reset failures; and discarded returns when a pool with `sizeFactor = 2` is full. On modern .NET, it also checks that 100,000 warmed lease/return cycles allocate zero bytes. Existing exception-path tests still compare the underlying buffer identities through the new metadata representation. See [development and testing](development-and-testing.md) for benchmark results.

## Other behavior relevant to integration

- A range facet with no values returns `default(TValue)` for both bounds, without a missing-data flag. An empty range cannot be directly distinguished from a valid `0..0` range.
- A probe with brands A and B confirmed that a keyword facet retains both brands with an `Any` filter on A, but shows only A with `All`. Filtering values 10, 20, and 30 to range `10..10` produced a facet maximum of 30.
- `SharedTokenEncoderStore` retains historical vocabulary. Retention follows from its design and needs measurement during long-running use with a changing vocabulary.
- Sort ties have no secondary ordering key. An application requiring stable pagination must define that order.

## Regression verification and further tests

The original 35 scoring/Include regression cases ran on .NET 10 before those fixes: 23 failed and 12 passed. After those fixes, all 212 tests passed on each of the four targets (848 executions).

For the exception-path and atomic return fixes, 16 of 20 new cases failed against the previous implementation on .NET 10; all 20 passed after the changes (1,102 complete-suite executions).

For copied-lease disposal, all seven initially ported Implicit cases failed against the previous Clara implementation on .NET 10. The completed change includes eight new cases on every target plus an allocation check on modern .NET. The complete suite passes 282 tests on `net472` and 285 on each of `net8.0`, `net9.0`, and `net10.0` (1,137 executions). The Release solution build passes with no warnings or errors.

Further test priorities include concurrent queries with custom analyzers and broader facet semantics for each mode. Add assertions for new contracts before changing the engine.
