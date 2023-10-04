﻿#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable SA1310 // Field names should not contain underscore

using BenchmarkDotNet.Attributes;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class QueryBenchmarks
    {
        private readonly string allTextSynonym;
        private readonly string? topBrand;
        private readonly decimal? maxPrice;
        private readonly Index<Product> index;
        private readonly Index<Product> index_x100;

        public QueryBenchmarks()
        {
            this.allTextSynonym = Guid.NewGuid().ToString("N");

            this.topBrand = Product.Items
                .Select(x => x.Brand)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            this.maxPrice = Product.Items
                .Max(x => x.Price);

            var synonymMapBindings =
                new[]
                {
                    new SynonymMapBinding(
                        new SynonymMap(
                            ProductMapper.Analyzer,
                            new Synonym[]
                            {
                                new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, this.allTextSynonym }),
                            }),
                        ProductMapper.Text),
                };

            this.index = IndexBuilder.Build(Product.Items, new ProductMapper(), synonymMapBindings);

            this.index_x100 = IndexBuilder.Build(Product.Items_x100, new ProductMapper(), synonymMapBindings);
        }

        [Benchmark]
        public void ComplexQuery_x100()
        {
            using var result = this.index_x100.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, this.allTextSynonym)
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Rating, SortDirection.Descending));

            ConsumeResult(result);
        }

        [Benchmark]
        public void ComplexQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, this.allTextSynonym)
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Rating, SortDirection.Descending));

            ConsumeResult(result);
        }

        [Benchmark]
        public void SearchQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, this.allTextSynonym));

            ConsumeResult(result);
        }

        [Benchmark]
        public void FilterQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1));

            ConsumeResult(result);
        }

        [Benchmark]
        public void FacetQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price));

            ConsumeResult(result);
        }

        [Benchmark]
        public void SortQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Sort(ProductMapper.Rating, SortDirection.Descending));

            ConsumeResult(result);
        }

        [Benchmark]
        public void BasicQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder());

            ConsumeResult(result);
        }

        private static void ConsumeResult(QueryResult<Product> result)
        {
            foreach (var document in result.Documents)
            {
                _ = document;
            }

            foreach (var facet in result.Facets)
            {
                if (facet is KeywordFacetResult keywordFacetResult)
                {
                    foreach (var value in keywordFacetResult.Values)
                    {
                        _ = value;
                    }
                }
                else if (facet is HierarchyFacetResult hierarchyFacetResult)
                {
                    foreach (var value in hierarchyFacetResult.Values)
                    {
                        foreach (var child in value.Children)
                        {
                            _ = child;
                        }
                    }
                }
                else if (facet is RangeFacetResult<decimal>)
                {
                }
            }
        }
    }
}
