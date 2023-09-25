# TODO

* [ ] Synonym maps
  * [ ] Extend `ExplicitMappingSynonym` to allow multiple mapped phrases
  * [ ] Evaluate `SynonymTree` multi token phrase handling with sythetic tokens
  * [ ] Evaluate `SynonymTree` implementation against document scoring
  * [ ] Write `SynonymTree` tests
  * [ ] Document synonym maps in `README.md`
* [ ] Analysis
  * [ ] Implement `Token`.`Delete`/`Insert`/`Replace` methods
  * [ ] Implement `GermanNormalizationTokenFilter` and other potential language specific filters
  * [ ] Write `Token` tests
  * [ ] Document `ITokenizer`, `IFilterToken` and `IAnalyzer` in `README.md`
* [x] Memory allocation reduction
  * [x] When consuming `KeywordFacetResult`.`Values`
  * [x] When consuming `HierarchyFacetResult`.`Values`
