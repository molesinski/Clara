# Repository Guidelines

## Project Structure & Module Organization

`Clara.slnx` contains the in-memory search engine and supporting projects:

- `src/Clara/`: core library, organized into `Analysis`, `Mapping`, `Querying`, `Storage`, and `Utils`.
- `src/Clara.Analysis.Snowball/`, `Clara.Analysis.Morfologik/`, and `Clara.Analysis.LuceneNet/`: language analysis implementations and integrations under `src/`.
- `src/Clara.Tests/`: xUnit tests and embedded `Product.json` fixtures.
- `src/Clara.Benchmarks/`: BenchmarkDotNet suites for tokenization, indexing, and querying.

Stop-word lists are embedded resources alongside analyzers. Shared build rules live in `Directory.Build.props`; packaging metadata lives in `Package.props`.

## Build, Test, and Development Commands

Run commands from the repository root. Install the .NET SDK selected by `global.json` (10.0.100, allowing newer feature bands).

- `dotnet restore Clara.slnx`: restore dependencies.
- `dotnet build Clara.slnx -c Release --no-restore`: compile all projects with analyzers enabled.
- `dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release`: run all test targets.
- `dotnet test src/Clara.Tests/Clara.Tests.csproj -c Release -f net10.0`: run only .NET 10 tests during development.
- `dotnet run --project src/Clara.Benchmarks -c Release -- --filter '*Querying*'`: benchmark querying performance.

The full test matrix targets .NET Framework 4.7.2 and .NET 8, 9, and 10; use Windows with the required runtimes for full validation.

## Coding Style & Naming Conventions

Follow `.editorconfig`: four spaces for C#, two for project/XML/JSON/YAML files, CRLF endings, and UTF-8 with BOM unless overridden. Use block-scoped namespaces, braces on separate lines, `var`, and `this.` qualification for instance members. Use PascalCase for types and members, `I`-prefixed interfaces, and camelCase fields and locals.

C# 14, nullable references, .NET analyzers, and StyleCop are enabled. Warnings fail builds. Respect the generated-code settings under `src/Clara.Analysis.Snowball/Snowball/`.

## Testing Guidelines

Follow existing `*Tests.cs` classes and PascalCase test methods. Use `[Fact]` for individual scenarios and `[Theory]` with `[InlineData]` for input variations. Add regression tests for changed behavior, including boundary cases. No numeric coverage threshold is configured. For performance-sensitive changes, compare relevant benchmarks.

## Commit & Pull Request Guidelines

Make repository changes on a dedicated branch and merge them into `master` through a pull request. Do not commit or push directly to `master`. Use the `codex/` prefix for agent-created branches. Merge only after the required CI checks pass.

Write repository documentation, code comments, commit messages, PR titles and descriptions, issue text, and release notes in English. Preserve language-specific test data, stop-word lists, and upstream content in their original language. Conversation with the user may follow the user's preferred language.

Use short, imperative commit subjects, following history: `Rename ObjectPool to ObjectPoolSlim` or `Move CI and NuGet publishing to GitHub Actions`. Keep changes focused. PR descriptions should explain behavior changes, link relevant issues, and report build/test results and applicable benchmark comparisons.

## Architecture Constraints

Indexes reside in memory and require complete rebuilds when source data changes. Dispose query results with `using` to return pooled buffers. Preserve these ownership and allocation conventions when extending search behavior.
