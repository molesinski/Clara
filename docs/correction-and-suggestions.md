# Token Correction and Phrase Suggestions

Use this chapter when a text field should tolerate misspelled or unfinished words, or when an application needs search phrase suggestions weighted by the documents that pass the same filters as its document queries.

## Token correction

`TextField` accepts an optional [`TokenCorrection`](../src/Clara/Mapping/TokenCorrection.cs). Correction compares query words with the words that occur in the indexed text, in their surface form: tokenized and normalized, but not stemmed. Matching against stems would reject typing errors in inflected forms, because a stem can differ from the typed word by more than the allowed distance.

```csharp
var surfaceAnalyzer = new Analyzer(new StandardTokenizer(), new LowerInvariantTokenFilter());

var text = new TextField<Product>(
    x => x.Title,
    new PorterAnalyzer(),
    tokenCorrection: new TokenCorrection(surfaceAnalyzer, TokenMatching.Default, variants: knownMisspellings));
```

- The correction analyzer must use the same tokenizer as the field (`ITokenizer.Equals`). Query and index terms are related to their surface forms by token position, so both analyzers must produce the same positions.
- While indexing, the field records which analyzed token each surface form produces. Only surface forms whose token has postings in the field are kept.
- `variants` maps known misspellings to canonical surface forms. Each side is normalized with the correction analyzer and must produce a single token; a variant is used only for query words of at least `minimumVariantLength` characters that do not occur in the field.

### Match modes

`QueryBuilder.Search(..., matchMode: ...)` selects the behavior. Correcting modes require a field with `TokenCorrection`.

| Mode | Term with an exact match | Term without an exact match | Last term, text not ending with whitespace |
| --- | --- | --- | --- |
| `Exact` (default) | Exact | No documents | Exact |
| `Correct` | Exact | Candidates with the highest boost (lowest cost) | Same as other terms |
| `CorrectPrefix` | Exact | Same as `Correct` | Exact plus every fuzzy and fuzzy prefix candidate |

A query word replaced by a synonym is corrected like any other word, using the word as typed: its candidates are added to the synonym's postings, and a word without synonym matches is corrected. A synonym that spans several query words is never corrected. A candidate contributes its token's postings multiplied by `TokenMatching.GetBoost(distance, isPrefix)`; within one term, a document keeps the best candidate (`Max`), so several similar words do not add up. Terms are then combined as before, with the position boost and the search aggregation.

### TokenMatching

[`TokenMatching`](../src/Clara/Mapping/TokenMatching.cs) defines the distance function and its limits:

| Member | Purpose |
| --- | --- |
| `Normalize(char)` | Character folding used for the first-character rule; candidates must start with the same folded character |
| `GetMaximumDistance(length)` | Largest accepted distance for a query word of the given length |
| `GetMaximumLengthDifference(length)` | Length window for full-word candidates and prefix lengths |
| `CalculateDistance(a, b, maximumDistance)` | Distance; may return any value above the maximum once it is exceeded |
| `GetBoost(distance, isPrefix)` | Score multiplier for a candidate |

`TokenMatching.Default` uses unit-cost Levenshtein distance ([`Levenshtein.CalculateDistance`](../src/Clara/Utils/Levenshtein.cs), which stops as soon as the distance is known to exceed the maximum): 0 edits up to 3 characters, 1 up to 6, and 2 above, with boost `1 / (1 + distance + (isPrefix ? 0.2 : 0))`. Applications that already rank suggestions with a weighted distance, for example one that charges less for a missing diacritic, should plug in that function so search and suggestions accept the same words.

## Phrase suggestions

[`PhraseField<TSource>`](../src/Clara/Mapping/PhraseField.cs) attaches weighted search phrases to documents, for example the phrases customers searched before choosing a product and how often they did. The field aggregates them per phrase, and a query suggests phrases using only the contributions of documents that pass its filters.

```csharp
public PhraseField<Product> Phrases { get; } =
    new(
        x => x.Phrases.Select(p => new PhraseWeight(p.Text, p.Count)),
        new Analyzer(new WhitespaceTokenizer(), new LowerInvariantTokenFilter()),
        validationField: Text,
        matching: matching,
        stopwords: stopwords,
        variants: knownMisspellings,
        aliases: phraseAliases);

using var result = index.Query(q => q
    .Filter(mapper.Brand, FilterMode.Any, "Alpha")
    .Suggest(mapper.Phrases, "choc", count: 10));

foreach (var suggestion in result.Suggestions)
{
    Console.WriteLine($"{suggestion.Text} {suggestion.HighlightedText} {suggestion.Score} {suggestion.Weight} {suggestion.Count}");
}
```

- `PhraseWeight(phrase, weight)` accepts zero, for phrases that should be suggested without popularity, such as brand names. Repeated phrases of one document add up.
- `validationField` keeps a document–phrase pair only if searching the phrase in that text field (`SearchMode.All`, `MatchMode.Exact`, synonyms included) finds the document. A suggestion then always finds at least one document under the same filters. The text field must belong to the same index; phrase fields are built after the other fields.
- `aliases` maps a phrase to an alternative wording that matches it, and `variants` maps known misspellings to phrase words; both are field settings because they describe phrases, not documents.

### Query semantics

`Suggest` runs after `Include`, `Search`, filters, and `Exclude`, on the final document set returned by the query. Facets can retain alternatives excluded by their own field filter; suggestions always respect every filter. For each phrase, its weight is the sum of the weights of the remaining documents that carry it, `Count` is the number of those documents (zero-weight carriers included), and the phrase is suggested only if at least one of them remains. `Count` counts documents that carry the phrase, not the documents a search for the phrase would find, which may be more. Without any restriction the precomputed totals are used. Because `Search` also restricts the set, send suggestions as a separate query when the typed text should not narrow the documents.

Empty or whitespace-only text returns phrases by weight. Otherwise every word of the text is matched against phrase words:

- exact and fuzzy words within `GetMaximumDistance`,
- fuzzy prefixes for the last word when the text does not end with whitespace,
- variants, exactly or as a prefix of the last word.

Words are intersected across the text. The score starts at 1,000,000 and subtracts the query word position, the matched word's position in the phrase, the phrase length in words, the distance, and one point each for a corrected or prefix match, a prefix match, and a stopword match. A phrase whose weight is zero, after filters when there are any, loses one more point per query word. Results are ordered by score, then weight, then by total weight and text, so filters change the order whenever they change weights. Weights are computed only for the score groups needed to fill `count`. `HighlightedText` marks the part of each phrase word that the query did not cover, using `highlightStart` and `highlightEnd` (`<b>` and `</b>` by default).

### Concurrency

A built index is read-only. Each query leases its own term source and working collections, so concurrent suggestions are safe as long as the analyzer's term sources are independent, as with the built-in analyzers.

### Upgrading

Existing source calls continue to use exact matching by default. Recompile consumers when upgrading: the optional parameters added to `TextField` constructors, `TextSearchExpression`, and `QueryBuilder.Search` change their binary signatures. To enable correction, configure `TokenCorrection` and select a correcting match mode. To enable suggestions, add a `PhraseField` to the mapper's `GetFields()` and rebuild the index. Dispose query results with `using` as before.

Tests: [TokenCorrectionTests.cs](../src/Clara.Tests/TokenCorrectionTests.cs), [PhraseFieldTests.cs](../src/Clara.Tests/PhraseFieldTests.cs).
