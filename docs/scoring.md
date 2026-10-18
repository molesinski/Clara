# Document Scoring

## Three calculation stages

1. Indexing combines the weights of token occurrences within a document through `IndexScoreAggregation`.
2. `Similarity.Transform` transforms the complete token-to-document map when the index is finalized.
3. Search combines the precomputed scores through `SearchScoreAggregation` and optional query position boosts.

Scores are `float` values. Changing the analyzer, stopword lists, synonyms, weights, or `Similarity` requires rebuilding the index. Search aggregation itself can be overridden through the `scoreAggregation` parameter of `QueryBuilder.Search`, subject to the limitation described below.

## Weight aggregation

[`ScoreAggregation`](../src/Clara/Mapping/ScoreAggregation.cs) provides:

| Variant | `Combine(current, value, boost)` |
| --- | --- |
| `Sum`, the default | `current + value * boost` |
| `Max` | `max(current, value * boost)` |

With the default index aggregation, repeated token occurrences add their weights. `Max` retains the largest occurrence weight. These settings are independent of search result aggregation. For domain text, a title can receive a higher weight than a description:

```csharp
new TextWeight(product.Title, weight: 3f, expandSynonyms: true);
```

`PositionBoost` has type `Func<Position, float>`. For example, `position => position.Start == 0 ? 2f : 1f` boosts the first query term. A multiword synonym can receive a position range instead of a single position. The callback should return finite weights that make sense for the application.

## Similarity variants

Source: [Similarity.cs](../src/Clara/Mapping/Similarity.cs). Let `s` be the token weight after index aggregation, `df` the number of documents containing the token, and `N` the number of documents in this field's length map. Documents that emit no tokens do not increase `N`.

### TF

`Similarity.TF` leaves the accumulated weights unchanged. It is useful for controlled aggregation comparisons because it does not account for document length or document frequency.

### TFIDF

```text
idf = ln(1 + N / df)
score = s * idf
```

### BM25

`Similarity.Default` is `Similarity.BM25(k1: 1.2f, b: 0.75f)`.

```text
idf = ln(1 + (N - df + 0.5) / (df + 0.5))
score = s * (k1 + 1)
        / (s + k1 * (1 - b + b * len / avgLen))
        * idf
```

`avgLen` is the average of the values supplied in `documentLengths`. The constructor rejects negative `k1` and `b`; it does not cap `b` at 1. Choose parameters deliberately and test them against application data.

## Notable behavior in the current code

[`TextFieldStoreBuilder`](../src/Clara/Storage/TextFieldStoreBuilder.cs) updates length with `length += IndexScoreAggregation.Combine(length, value.Weight)`. With `Sum` and unit weights, this produces `1, 3, 7, 15` instead of the token count. Supplying these lengths to BM25 affects normalization and can substantially lower scores for long text. Do not interpret the current `len` as a simple token count.

In [`TextDocumentStore`](../src/Clara/Storage/TextDocumentStore.cs), an aggregation override supplied by the query applies when combining terms. Inside a synonym phrase, the implementation still uses the field's aggregation. Alternatives in `PhraseGroup` always combine through `Max`. With mapping `alias => red blue`, `Similarity.TF`, and document `red blue`, a query override of `Max` produces score 2 under the field's default `Sum`, while a plain `red blue` query with `Max` produces 1.

These are verified behaviors of the current implementation that need to be resolved before changing ranking. Reproductions and suggested tests are in the [implementation notes](implementation-notes.md).

## Extending ranking

A custom `Similarity` class implements `Transform`, modifying weights in the supplied dictionaries. A custom `ScoreAggregation` implements `Combine`. Check token weights, normalization, hit ordering, repetitions, multivalued fields, and synonyms separately. Testing only whether a document appears will not detect scoring regressions.
