# Indexing and the Field Model

## Mapper contract

[`IIndexMapper<TSource, TDocument>`](../src/Clara/Mapping/IIndexMapper.cs) separates the input object from the object returned to the caller:

- `GetFields()` returns the index's field definitions.
- `GetDocumentKey(item)` returns the document's string key.
- `GetDocument(item)` returns the object stored in the result document map.

`IIndexMapper<TSource>` is shorthand for identical source and result types. Format numeric identifiers using the invariant culture, as the test mapper does. The library rejects duplicate keys, `null` source items, duplicate field instances, and fields built for the wrong source type.

Keep field definitions in mapper properties. Do not create fresh fields on each `GetFields()` call or create a separate, apparently equivalent mapper for queries. Object identity identifies a field, rather than a name. `GetDocument()` does not create a deep copy: the returned object remains an application object.

## Available fields

| Type | Values | Operations |
| --- | --- | --- |
| `TextField<TSource>` | Text, a collection of strings, or `TextWeight` values | Search and ranking |
| `KeywordField<TSource>` | One or more labels | Filtering and faceting |
| `PhraseField<TSource>` | Weighted search phrases (`PhraseWeight`) | Phrase suggestions weighted by the filtered documents |
| `HierarchyField<TSource>` | One or more encoded paths | Node filtering and hierarchical facets |
| `RangeField<TSource, TValue>` | A nullable value or a collection of comparable values | Range filtering, min/max, and sorting |
| `Int32Field`, `DoubleField`, `DecimalField`, `DateTimeField` | Convenient range specializations | Same as `RangeField` |

For non-text fields, explicitly enable the capabilities you need: `isFilterable`, `isFacetable`, and `isSortable`. The constructor requires at least one supported operation. These flags also control memory use: the store builds only the required relationships and arrays.

### Text and weights

A single text field can collect a title, description, and category, enabling search across multiple domain properties. Separate text fields can represent different languages, but a query selects one of them.

`TextWeight(text, weight, expandSynonyms)` sets a positive weight and controls synonym expansion for a specific value. Plain text receives weight `1` with expansion enabled. Empty and whitespace-only text is skipped. See [scoring](scoring.md) for aggregation details.

Document length uses `IndexScoreAggregation` across the weights of all emitted tokens: `Sum` adds each weight once, while `Max` retains the largest weight. Expanded synonym tokens participate in this calculation; disabling expansion uses the analyzer's original tokens. The resulting length is used by BM25 during finalization.

### Keywords and hierarchies

Keyword fields do not run an analyzer. Values are trimmed, and empty or `null` values are skipped. Comparisons are case-sensitive: `Alpha` and `alpha` are different values. A repeated label within one document does not increase its facet count.

For input `Home/Kitchen/Mugs`, separator `/`, and root `all`:

- `Identifiers` creates nodes `all`, `Home`, `Kitchen`, and `Mugs`.
- `Path` creates nodes `all`, `Home`, `Home/Kitchen`, and `Home/Kitchen/Mugs`.

The document also belongs to its ancestors. In `Identifiers` mode, a node name is shared across branches, so identifiers must be globally unambiguous. In `Path` mode, paths distinguish repeated leaf names. Do not include the root in the input value if the builder is supposed to add it.

### Ranges

Values outside the field's `MinValue`–`MaxValue` bounds are skipped during indexing. `null` means no value. For a multivalued field, a filter matches a document if at least one value falls within the requested range. Ascending sorting uses the document's minimum; descending sorting uses its maximum.

## Build stages

1. The static `IndexBuilder.Build` method locks `TokenEncoderStore.SyncRoot` for the entire build.
2. The builder retrieves the fields and creates their corresponding `FieldStoreBuilder` instances.
3. The document key is encoded as an integer, and the document is added to `documentMap`.
4. Each field builder reads the document's values and updates its own structures.
5. `Build()` finalizes the encoder and field stores, then creates `Index<TDocument>`. Phrase fields with a validation field are finalized after the other fields, because they search it.

`IndexBuilder<TSource, TDocument>` allows manual calls to `Index(item)` followed by `Build()`. Both operations are prohibited after finalization. This variant does not automatically acquire the static method's lock; when using a shared encoder store, the application must synchronize the entire build cycle.

## Storage structures

| Field | Structures |
| --- | --- |
| Text | Token → document → `float`; document lengths used when finalizing scores; with `TokenExpansion`, surface form → token grouped by first character |
| Keyword | Token → document set for filters; document → token set for facets |
| Hierarchy | Keyword relationships plus parent → children |
| Range | Sorted document–value pairs for filters; document → min/max for facets and sorting |
| Phrase | Phrase → sorted documents and weights; phrase word → phrase → position, grouped by first character |

The encoder is shared by document keys and text values within an index. Do not treat its integer identifiers as public domain identifiers. The [memory chapter](memory-and-concurrency.md) explains the choice between `SharedTokenEncoderStore` and `InstanceTokenEncoderStore`.

Source: [IndexBuilder.cs](../src/Clara/IndexBuilder.cs), [HierarchyFieldStoreBuilder.cs](../src/Clara/Storage/HierarchyFieldStoreBuilder.cs), [RangeFieldStoreBuilder.cs](../src/Clara/Storage/RangeFieldStoreBuilder.cs).
