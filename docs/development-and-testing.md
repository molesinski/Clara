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
| `PoolOwnershipTests.cs` | Relevant buffer identities after exceptions and result disposal, partial facet cleanup, and filter replacement |
| `QueryResultLifetimeTests.cs` | Independent result lifetimes, disposal guards, query reuse, and concurrent queries with failure isolation |
| `ObjectPoolSlimTests.cs` | Concurrent returns without overwritten instances and reset before reuse |
| `ObjectPoolSlimLeaseTests.cs` | Default lease disposal and allocation-free warmed reuse |

`Product.json` is an embedded resource; tests do not download products from the network. No coverage threshold or coverage collector is configured. Choose test cases according to behavior, not just line counts.

`PoolOwnershipTests` inspects only the pools relevant to each ownership regression through reflection and disables collection parallelization so other tests cannot borrow a buffer during a snapshot. Result independence and repeated disposal are tested through the public API in `QueryResultLifetimeTests`.

Allocation checks run only on modern .NET and carry the `Category=Performance` trait. The default test command still includes them. To run functional tests separately, use `--filter "Category!=Performance"`; to run allocation checks alone, use `--filter "Category=Performance"` with `-f net10.0` (or .NET 8/9). These checks guard allocation budgets; timing measurements belong in the benchmarks below.

The [quick start](quick-start.md) and [README](../README.md) retain the introductory query and result-reading examples. Behavioral coverage lives in the query, facet, and result lifetime tests.

The [Snowball maintenance guide](updating-snowball.md) describes generation and full upstream corpus verification.

## Benchmarks

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Querying*'
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Indexing*'
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Tokenization*'
```

BenchmarkDotNet 0.15.8 reports time and memory. `IndexingBenchmarks` compares shared and instance encoder stores; `QueryingBenchmarks` covers search, filters, facets, sorting, and `x100` dataset variants; `TokenizationBenchmarks` compares analyzers. The program configures Markdown export. Results are written to the ignored `BenchmarkDotNet.Artifacts/` directory.

The index and query benchmarks use the bundled 100-product dataset. Variants with an `x100` suffix repeat it 100 times.

Compare before-and-after results on the same machine, runtime, and data. The number of unique tokens, result size, and number of facet values may affect cost more than document count alone. Do not infer performance from unit test duration.

### Pool ownership and atomic returns

For the pool ownership and atomic return changes, BenchmarkDotNet `ShortRun` on .NET 10.0.13 / Intel Core i9-14900K produced:

| Benchmark | Before | After | Allocated before / after |
| --- | --- | --- | --- |
| `QueryComplex` | 6.311 us | 6.480 us | 864 B / 864 B |
| `QuerySearch` | 2.470 us | 2.580 us | 352 B / 352 B |

These are three measured iterations after three warmups, with one launch. Allocations are unchanged; observed mean times increased by about 2.7% and 4.5%, respectively. The 99.9% confidence intervals overlap, so this short run does not establish a precise throughput change. Reports are retained locally under ignored `artifacts/pool-before/` and `artifacts/pool-after/`. Reproduce with:

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*QueryingBenchmarks.QuerySearch' '*QueryingBenchmarks.QueryComplex' --job short --artifacts artifacts/pool-after
```

### Pool simplification

The simple lease stores pool and instance references without shared version metadata. This comparison measures the versioned implementation at `0150aab` against the simplified implementation, retaining atomic pool returns in both. BenchmarkDotNet 0.15.8 ran on .NET 10.0.13 / Intel Core i9-14900K with one launch, five warmups, and ten measured iterations per benchmark.

| Benchmark | Versioned lease | Simple lease | Allocated before / after |
| --- | --- | --- | --- |
| `LeaseAndReturn` | 24.38 ns | 14.59 ns | 0 B / 0 B |
| `QueryComplex` | 12.590 us | 12.415 us | 864 B / 864 B |
| `QuerySearch` | 5.547 us | 4.739 us | 352 B / 352 B |

These local, single-thread measurements show lower mean times for the pool cycle and search. Timing variability limits the precision of the comparison; `QueryComplex` has overlapping 99.9% confidence intervals and does not establish an improvement. Warmed allocations are unchanged. New pooled instances no longer require a separate metadata object. See [ownership rules](memory-and-concurrency.md#pools-and-slim-collections) for the single-disposal contract.

Reports are retained locally under ignored `artifacts/pool-simplification-before/` and `artifacts/pool-simplification-after/`. Reproduce with:

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*QueryingBenchmarks.QuerySearch' '*QueryingBenchmarks.QueryComplex' '*ObjectPoolSlimBenchmarks*' --job short --warmupCount 5 --iterationCount 10 --artifacts artifacts/pool-simplification-after
```

### Snowball 3.2.0

BenchmarkDotNet ShortRun on .NET 10.0.13 measured the existing English and Polish analyzer phrases before and after the Snowball 3.2.0 update:

| Analyzer | Before | After | Allocated before / after |
| --- | --- | --- | --- |
| English | 889.5 ns | 895.0 ns | 0 B / 0 B |
| Polish | 526.2 ns | 581.9 ns | 0 B / 0 B |

The short run has wide timing uncertainty, particularly for Polish; these numbers do not establish a speed improvement or regression.

```powershell
dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*TokenizationBenchmarks.EnglishAnalyzer' '*TokenizationBenchmarks.PolishAnalyzer' --job short
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

Follow [AGENTS.md](../AGENTS.md) and [.editorconfig](../.editorconfig), including the local exceptions for generated Snowball code.

## Packaging and CI

[`azure-pipelines.yml`](../azure-pipelines.yml) runs on Windows, restores and builds the libraries and tests, runs tests with TRX output, packages the core, Morfologik, and Snowball, then publishes to NuGet through a configured connection. The pipeline does not run benchmarks. `trigger: none` and `pr: none` disable YAML push and PR triggers; do not assume every change is automatically validated.

To prepare a package locally after building:

```powershell
dotnet pack src/Clara/Clara.csproj -c Release --no-build -o artifacts/packages
```

[`Package.props`](../Package.props) defines metadata, the MIT license, and inclusion of the main README. `docs/` is not currently included in NuGet packages. Publishing packages is a separate release operation; ordinary change validation does not require a push.
