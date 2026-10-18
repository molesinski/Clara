# Development and Testing

## Environment

[`global.json`](../global.json) selects SDK 10.0.100 with `rollForward=latestFeature`. Shared settings enable C# 14, nullable references, implicit usings, .NET and StyleCop analyzers, and warnings as errors. The full test suite requires Windows, .NET Framework 4.7.2 support, and the .NET 8, 9, and 10 runtimes.

| Projects | Target frameworks |
| --- | --- |
| Core and the three analyzer projects | `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0`, `net10.0` |
| `Clara.Tests` | `net472`, `net8.0`, `net9.0`, `net10.0` |
| `Clara.Benchmarks` | `net10.0` |

`Clara` and Snowball declare `IsTrimmable=true`. The other language projects do not. When changing APIs or dependencies, check every target, especially `#if` branches supporting .NET Standard 2.0.

## Commands from the repository root

```powershell
dotnet restore Clara.slnx
dotnet build Clara.slnx -c Release --no-restore
dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release --no-build
```

When working on one area:

```powershell
dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release -f net10.0
dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release -f net10.0 --filter FullyQualifiedName~SynonymMapTests
```

Use `--no-build` after a current build in the same configuration; `--no-restore` requires dependencies to have been restored. The library has no development server. See the [quick start](quick-start.md) for an executable example.

## Test map

The project uses xUnit 2.9.3, Microsoft.NET.Test.Sdk 18.11.0, and Visual Studio runner 4.0.1.

| File under `src/Clara.Tests` | Coverage |
| --- | --- |
| `TokenTests.cs` | Token buffer editing operations and invalid arguments |
| `AnalysisTests.cs` | Tokenization and English analysis compared with Lucene |
| `SnowballTests.cs` | Snowball 3.2.0 rules, pooled buffer reuse, empty and long tokens, and allocation budgets |
| `MorfologikTests.cs` | Preferred lemmas ending in `y`, dictionary fallbacks, context limitations, related forms, and buffer reuse |
| `SynonymMapTests.cs` | Synonym matching, position backtracking, and rule parsing |
| `QueryTests.cs` | Composite queries, search, filters, and some facet behavior |
| `ScoringTests.cs` | Weighted document lengths, BM25 scores, long documents, synonym aggregation overrides, alternatives, and position boosts |
| `IncludeTests.cs` | Current document key validation, shared encoder history, empty and mixed selections, and interaction with search and facets |
| `PoolOwnershipTests.cs` | Buffer identities after repeated exceptions, partial facet cleanup, filter replacement, and successful result ownership |
| `ObjectPoolSlimTests.cs` | Concurrent returns without overwritten instances and reset before reuse |
| `GettingStartedTests.cs` | Integration and result-reading example |

`Product.json` is an embedded resource; tests do not download products from the network. No coverage threshold or coverage collector is configured. Choose test cases according to behavior, not just line counts.

The Morfologik heuristic is covered by 28 regression cases using the bundled `Morfologik.Polish 2.1.7` dictionary. See [lemma selection](text-analysis.md#morfologik-lemma-selection) for the search policy and its limitations.

After the pool fixes, the full suite passes 274 tests on `net472` and 276 on each of `net8.0`, `net9.0`, and `net10.0` (1,102 executions), with no failures or skips. The Release solution build passes without warnings or errors. Of the 20 added pool cases, 16 failed before the fixes on .NET 10. `PoolOwnershipTests` uses reflection only in tests to compare pooled object identities and disables collection parallelization so another test cannot borrow a buffer during a snapshot.

Verification on October 18, 2026, after the three fixes: `dotnet build Clara.slnx -c Release --no-restore` succeeded with SDK 10.0.402 and no warnings or errors. `dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release --no-build --no-restore` passed all 212 tests on each of the four targets, for 848 executions in total, with no failures or skips. The 35 new regression cases first ran against the previous implementation on .NET 10: 23 failed and 12 passed. All 35 pass with the fixes. See [implementation notes](implementation-notes.md) for the corrected contracts. These checks are not performance measurements.

## Benchmarks

After the Snowball update, the full suite passes 226 cases on `net472` and 228 on each modern .NET target (910 executions). The additional allocation checks require modern .NET. The [Snowball maintenance guide](updating-snowball.md) describes the separate Docker generator and full upstream corpus verifier retained under `tools/`.

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Querying*'
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Indexing*'
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Tokenization*'
```

BenchmarkDotNet 0.15.8 reports time and memory. `IndexingBenchmarks` compares shared and instance encoder stores; `QueryingBenchmarks` covers search, filters, facets, sorting, and `x100` dataset variants; `TokenizationBenchmarks` compares analyzers. The program configures Markdown export. Results are written to the ignored `BenchmarkDotNet.Artifacts/` directory.

Compare before-and-after results on the same machine, runtime, and data. The number of unique tokens, result size, and number of facet values may affect cost more than document count alone. Do not infer performance from unit test duration.

For the pool ownership and atomic return changes, BenchmarkDotNet `ShortRun` on .NET 10.0.13 / Intel Core i9-14900K produced:

| Benchmark | Before | After | Allocated before / after |
| --- | --- | --- | --- |
| `QueryComplex` | 6.311 us | 6.480 us | 864 B / 864 B |
| `QuerySearch` | 2.470 us | 2.580 us | 352 B / 352 B |

These are three measured iterations after three warmups, with one launch. Allocations are unchanged; observed mean times increased by about 2.7% and 4.5%, respectively. The 99.9% confidence intervals overlap, so this short run does not establish a precise throughput change. Reports are retained locally under ignored `artifacts/pool-before/` and `artifacts/pool-after/`. Reproduce with:

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*QueryingBenchmarks.QuerySearch' '*QueryingBenchmarks.QueryComplex' --job short --artifacts artifacts/pool-after
```

## Extending the project

| Change | Location and verification |
| --- | --- |
| New filter | `ITokenFilter`, explicit `next` invocation; test buffers, empty tokens, and filter order |
| New analyzer | `IAnalyzer` or `Analyzer` composition; test tokens, stopwords, keywords, and independently used sources |
| New synonym rules | `SynonymMapTests`; check indexing and querying, shorter/longer prefixes, and scoring |
| New scoring | `Similarity` or `ScoreAggregation`; assert numeric scores and ranking, not only matches |
| New field type | Definition in `Mapping`, builder and store in `Storage`, expressions and results in `Querying`; test every capability flag |
| Slim structure changes | Test empty and growing collections, reset behavior, and lifetimes; benchmark allocations |

Style is defined in [AGENTS.md](../AGENTS.md) and [.editorconfig](../.editorconfig): four spaces in C#, block-scoped namespaces, `this.`, `var`, and PascalCase for types and methods. Preserve local exceptions for generated Snowball code.

## Packaging and CI

[`azure-pipelines.yml`](../azure-pipelines.yml) runs on Windows, restores and builds the libraries and tests, runs tests with TRX output, packages the core, Morfologik, and Snowball, then publishes to NuGet through a configured connection. The pipeline does not run benchmarks. `trigger: none` and `pr: none` disable YAML push and PR triggers; do not assume every change is automatically validated.

To prepare a package locally after building:

```powershell
dotnet pack src/Clara/Clara.csproj -c Release --no-build -o artifacts/packages
```

[`Package.props`](../Package.props) defines metadata, the MIT license, and inclusion of the main README. `docs/` is not currently included in NuGet packages. Publishing packages is a separate release operation; ordinary change validation does not require a push.

Commit history uses short imperative subjects, such as `Separate indexing and search score aggregations`. In a PR, describe the behavior change, verification, test targets, and benchmark results when performance is affected.
