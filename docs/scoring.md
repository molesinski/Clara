# Document Scoring

## Three calculation stages

1. Indexing combines the weights of token occurrences within a document through `IndexScoreAggregation`.
2. `Similarity.Transform` transforms the complete token-to-document map when the index is finalized.
3. Search combines the precomputed scores through `SearchScoreAggregation` and optional query position boosts.

Scores are `float` values. Changing the analyzer, stopword lists, synonyms, weights, or `Similarity` requires rebuilding the index. Search aggregation itself can be overridden through the `scoreAggregation` parameter of `QueryBuilder.Search`. The effective aggregation applies to individual tokens, tokens within synonym phrases, and the combination of query terms.

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

## Document length and synonym scoring

[`TextFieldStoreBuilder`](../src/Clara/Storage/TextFieldStoreBuilder.cs) updates length by assigning `IndexScoreAggregation.Combine(length, value.Weight)` once per emitted token. Length spans all values of the field in a document. With `Sum`, it is the sum of emitted token weights, so unit weights produce the token count. With `Max`, it is the largest emitted token weight, not a token count. For example, `red red` at weight 2 and `blue` at weight 3 produce length 7 under `Sum` and 3 under `Max`.

Synonym expansion contributes the tokens actually emitted for indexing. `TextWeight.ExpandSynonyms=false` instead contributes the analyzer's original tokens. Empty values and text that emits no tokens do not add an entry to the document length map. Custom index aggregations are applied in token order, starting from zero.

In [`TextDocumentStore`](../src/Clara/Storage/TextDocumentStore.cs), the query's aggregation override takes precedence over the field's setting, including inside synonym phrases. If the override is omitted, the field's `SearchScoreAggregation` is used. Alternatives in `PhraseGroup` always combine through `Max`, preventing multiple matching representations from adding their scores. A position boost is applied when the completed term score is combined into the query result.

With mapping `alias => red blue`, `Similarity.TF`, and document `red blue`, a query override of `Max` produces score 1 even when the field uses `Sum`. Conversely, an override of `Sum` produces score 2 when the field uses `Max`. [ScoringTests.cs](../src/Clara.Tests/ScoringTests.cs) covers these cases, default fallback, alternative phrases, and position boosts.

## Extending ranking

A custom `Similarity` class implements `Transform`, modifying weights in the supplied dictionaries. A custom `ScoreAggregation` implements `Combine`. Check token weights, normalization, hit ordering, repetitions, multivalued fields, and synonyms separately. Testing only whether a document appears will not detect scoring regressions.
