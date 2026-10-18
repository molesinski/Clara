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
| [Development and testing](development-and-testing.md) | Environment, commands, extensions, and publishing |
| [Updating Snowball](updating-snowball.md) | Reproducible Docker generation, allocation adaptations, and corpus verification |
| [Implementation notes](implementation-notes.md) | Verified behavior to consider when making changes |

For library integration, start with the example, field model, and queries. Before changing the engine, also read the memory chapter and scoring notes.

## Scope and baseline

This documentation describes the current code, including the document length, synonym aggregation, `Include`, and pool ownership corrections summarized in [implementation notes](implementation-notes.md). The initial analysis used revision `c3073b3` on October 18, 2026. Links point to source files in this repository. The main [README](../README.md) introduces field mapping and text analysis; these chapters provide the detailed contracts and implementation context.

The repository does not provide an HTTP server or a separate search service. The application embedding the library is responsible for loading data, refreshing the index, and presenting results. Contributor guidelines are in [AGENTS.md](../AGENTS.md).
