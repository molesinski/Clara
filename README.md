# Clara

[![Build Status](https://dev.azure.com/molesinski/Clara/_apis/build/status/Clara?branchName=master)](https://dev.azure.com/molesinski/Clara/_build/latest?definitionId=1&branchName=master)
[![Latest version](https://img.shields.io/nuget/v/Clara.svg)](https://www.nuget.org/packages/Clara)
[![License MIT](https://img.shields.io/github/license/molesinski/Clara.svg)](https://github.com/molesinski/Clara/blob/master/LICENSE)

Simple, yet feature complete, in memory search engine.

## Highlights

- Inspired by commonly known Lucene design
- Fast in memory searching
- Low memory allocation for search execution
- Stemmers and stopwords handling for 30 languages
- Text, keyword, hierarchy and range (any comparable structure values) fields
- Synonym maps with multi word synonym support
- Fully configurable and extendable text analysis pipeline
- Searching with BM25 weighted document scoring
- Filtering on any field type by values or range
- Faceting without restricting facet value list by filtered values
- Result sorting document score or range fields
- Fluent query builder

## Getting started

For example usage check sample test or benchmark project contents.

## Supported languages

### Internally

Porter

### via Snowball

Arabic, Armenian, Basque, Catalan, Danish, Dutch, English, Finnish, French, German, Greek, Hindi, Hungarian, Indonesian, Irish, Italian, Lithuanian, Nepali, Norwegian, Portuguese, Romanian, Russian, Serbian, Spanish, Swedish, Tamil, Turkish, Yiddish

### via Morfologik

Polish

## License

Released under the MIT License
