# TODO

* [ ] Synonym maps
  * [ ] Extend `ExplicitMappingSynonym` to allow multiple mapped phrases
  * [ ] Evaluate `SynonymGraph` multi token phrase handling with sythetic tokens
  * [ ] Evaluate `SynonymGraph` implementation against document scoring
  * [ ] Write `SynonymGraph` tests
  * [ ] Document synonym maps in `README.md`
* [ ] Analysis
  * [ ] Implement `Token`.`Delete`/`Insert`/`Replace` methods
  * [ ] Implement `GermanNormalizationTokenFilter` and other potential language preprocessing filters
  * [ ] Write `Token` tests
  * [ ] Document `ITokenizer`, `IFilterToken` and `IAnalyzer` in `README.md`
* [ ] Memory allocation reduction
  * [ ] When consuming `KeywordFacetResult`.`Values`
  * [ ] When consuming `HierarchyFacetResult`.`Values`
