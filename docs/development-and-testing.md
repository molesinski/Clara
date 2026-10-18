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

The project uses xUnit 2.9.3, Microsoft.NET.Test.Sdk 18.0.1, and Visual Studio runner 3.1.5.

| File under `src/Clara.Tests` | Coverage |
| --- | --- |
| `TokenTests.cs` | Token buffer editing operations and invalid arguments |
| `AnalysisTests.cs` | Tokenization and English analysis compared with Lucene |
| `SynonymMapTests.cs` | Synonym matching, position backtracking, and rule parsing |
| `QueryTests.cs` | Composite queries, search, filters, and some facet behavior |
| `GettingStartedTests.cs` | Integration and result-reading example |

`Product.json` is an embedded resource; tests do not download products from the network. No coverage threshold or coverage collector is configured. Choose test cases according to behavior, not just line counts.

Verification on October 18, 2026, for `c3073b3`: `dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release --no-restore` succeeded with SDK 10.0.402. All 177 tests passed on each of the four targets, for 708 executions in total, with no failures or skips. Additional small probes confirmed the behavior described in the [implementation notes](implementation-notes.md). This was not a performance measurement.

## Benchmarks

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Querying*'
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Indexing*'
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Tokenization*'
```

BenchmarkDotNet 0.15.8 reports time and memory. `IndexingBenchmarks` compares shared and instance encoder stores; `QueryingBenchmarks` covers search, filters, facets, sorting, and `x100` dataset variants; `TokenizationBenchmarks` compares analyzers. The program configures Markdown export. Results are written to the ignored `BenchmarkDotNet.Artifacts/` directory.

Compare before-and-after results on the same machine, runtime, and data. The number of unique tokens, result size, and number of facet values may affect cost more than document count alone. Do not infer performance from unit test duration.

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
