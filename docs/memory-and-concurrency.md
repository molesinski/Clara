# Memory and Concurrency

## Object ownership

| Object | Usage rule |
| --- | --- |
| `Index<TDocument>` | Structures ready for reading; no `Dispose` or update operations |
| `QueryBuilder` | Stateful and single-use; dispose it if it has not transferred its query through `ToQuery()` |
| `Query` | Holds pooled collections and filters; its direct owner calls `Dispose()` |
| `QueryResult<TDocument>` | Owns document and facet results; always use `using` |
| Collections, enumerators, and facet children | Views valid during the result's lifetime |
| `ITokenTermSource` | Stateful analysis; one enumeration at a time |
| `ObjectPoolSlimLease<T>` | One owner; dispose each acquired lease exactly once, including across copies |

`Index.Query(QueryBuilder)` consumes the builder and disposes the resulting `Query`. `Index.Query(Action<QueryBuilder>)` manages both objects. `Index.Query(Query)` leaves ownership of the supplied query with the caller. In every variant, the returned result must be disposed.

`Query.AddFilter` takes ownership of an added filter. Replacing it for the same field disposes the previous filter; re-adding the identical instance does not dispose it. Do not share filter instances between queries. The higher-level builder rejects duplicate filters.

Do not return `result.Documents.Select(...)` from a method that exits the result's `using` block. Materialize the result with `ToArray()` inside the block. For facets, copy scalar values and children into your own DTOs; copying a view object does not transfer ownership of its buffer.

## Pools and Slim collections

[`SharedObjectPools`](../src/Clara/SharedObjectPools.cs) leases identifier sets, score dictionaries, result lists, and facet values, among other structures. `DocumentSet`, `DocumentList`, and `DocumentScoring` abstract the distinction between reading existing structures and owning leased temporary structures.

`DictionarySlim` provides `ref` access to values, reducing repeated lookups during increments. Do not retain such a reference across an operation that can resize the dictionary. These collections are working data structures; do not treat them as general-purpose concurrent collections.

[`ObjectPoolSlim`](../src/Clara/Utils/ObjectPoolSlim.cs) has `Environment.ProcessorCount * 5 * sizeFactor` slots. An empty pool creates a new object, and a full pool may discard a returned object. This is not a limit on concurrent queries. Resetting typically clears contents while allowing allocated capacity to be reused.

Both leasing and returning use atomic compare-and-exchange operations. A return resets the instance before publishing it into an empty slot; competing returns cannot overwrite one another's occupied slots. This protects the pool, not concurrent access to an individual leased collection.

`ObjectPoolSlimLease<T>` is a readonly struct holding the pool and instance references. It has no shared disposal state or per-instance metadata allocation. Disposing a default lease is a no-op.

Keep one logical owner and use `using`. Dispose each acquired lease exactly once, including across copied or boxed values. Repeated disposal can publish the same instance more than once or reset it while another caller uses it. Do not access `Instance` after disposal. The higher-level `Query` and `QueryResult` types retain their own disposal guards.

The default resetter calls `IResettable.Reset()` when supported; a custom resetter replaces this behavior. If resetting throws, the instance is not published back to the pool. Prefer non-throwing resetters and do not retry disposal after a reset failure.

## Choosing an encoder store

| Store | Behavior | Consequence |
| --- | --- | --- |
| `SharedTokenEncoderStore.Default` | Retains a shared builder and copies encoder structures during finalization | Reuses the vocabulary but retains historical values and serializes builds through a shared lock |
| `new InstanceTokenEncoderStore()` | Creates a separate builder and transfers its structures to the index | The vocabulary belongs to the current build; useful for independent indexes and controlling retention |

The shared store does not remove tokens belonging to documents that disappear from subsequent indexes. When rebuilding frequently with new identifiers, measure vocabulary size and memory use. Shared encoder identifiers do not imply shared document lists or field weights.

Sources: [SharedTokenEncoderStore.cs](../src/Clara/Storage/SharedTokenEncoderStore.cs), [InstanceTokenEncoderStore.cs](../src/Clara/Storage/InstanceTokenEncoderStore.cs).

## Concurrent use and rebuilding

Queries read the completed index's stores, while working collections and phrase sources are leased separately. The application should create a separate builder, query, and result for each invocation. Do not concurrently modify `Include`/`Exclude` sources, callback state, or result objects. Custom analyzers and filters must provide independent processing state or synchronization.

A safe application refresh pattern is to build a new index in the background, then atomically replace the reference. If this also creates new field instances, replace the index and mapper together as one state object. Each query should capture one index–mapper pair and use it throughout execution.

Do not transfer an old `Query` to a new index. The index does not deep-copy `TDocument`, so changing a domain object can alter the displayed data without updating search structures. Use immutable projections or manage their ownership explicitly.

## Exceptions and ownership transfer

Query execution always disposes its document result builder. Before a successful result is constructed, it also owns the output scores, document list, and facets: a failure returns those buffers, disposing completed facets before their containing list. Text search separately returns its destination score dictionary if analysis or a scoring callback throws. A filter value collection returns its set if construction fails during enumeration.

Successful execution transfers output ownership to `QueryResult`; these buffers remain leased until that result is disposed. Do not put a transferring lease in an unconditional `using` scope that ends before the consumer finishes. Keep mutable `DocumentResultBuilder` cleanup in `finally` on the original variable, avoiding copies of its ownership state.
