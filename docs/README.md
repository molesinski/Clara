# Clara Developer Documentation

Clara is a C# library for searching an index held in process memory. It combines text analysis, document ranking, filtering, faceting, and sorting. The index is built from the complete dataset and must be rebuilt when that dataset changes.

## Chapters

| Chapter | Purpose |
| --- | --- |
| [Quick start](quick-start.md) | A complete mapper, index, and query example |
| [Architecture](architecture.md) | Modules, dependencies, and data flow |
| [Indexing and the field model](indexing.md) | Document keys, schema, field types, and stores |
| [Queries and facets](queries.md) | Search, filter, facet, and sorting semantics |
| [Text analysis and synonyms](text-analysis.md) | Tokenization, filters, languages, and synonym rules |
| [Scoring](scoring.md) | Weights, aggregation, TF, TFIDF, and BM25 |
| [Memory and concurrency](memory-and-concurrency.md) | Result ownership, pooling, and index replacement |
| [Development and testing](development-and-testing.md) | Environment, commands, benchmarks, extensions, and publishing |
| [GitHub Actions and releases](github-actions-and-releases.md) | CI, shared package versions, NuGet Trusted Publishing setup, and release recovery |
| [Updating Snowball](updating-snowball.md) | Reproducible Docker generation, allocation adaptations, and corpus verification |

For library integration, start with the example, field model, and queries. Before changing the engine, also read the memory chapter and scoring notes.

## Scope

The main [README](../README.md) covers library usage, field mapping, text analysis, and a catalog example. These chapters provide developer reference material for implementation contracts, maintenance, and testing.

The repository does not provide an HTTP server or a separate search service. The application embedding the library is responsible for loading data, refreshing the index, and presenting results. Contributor guidelines are in [AGENTS.md](../AGENTS.md).
