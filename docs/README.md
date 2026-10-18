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
| [Implementation notes](implementation-notes.md) | Verified behavior to consider when making changes |

For library integration, start with the example, field model, and queries. Before changing the engine, also read the memory chapter and scoring notes.

## Scope and baseline

This documentation describes the current code, including the document length, synonym aggregation, and `Include` corrections summarized in [implementation notes](implementation-notes.md). The initial analysis used revision `c3073b3` on October 18, 2026. Links point to source files in this repository. Current code contracts take precedence over older examples in the main README; for example, the builder's range filter uses `from` and `to` parameters, and range facets expose `MinValue` and `MaxValue`.

The repository does not provide an HTTP server or a separate search service. The application embedding the library is responsible for loading data, refreshing the index, and presenting results. Contributor guidelines are in [AGENTS.md](../AGENTS.md).
