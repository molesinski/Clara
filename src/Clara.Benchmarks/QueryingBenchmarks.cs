#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable SA1310 // Field names should not contain underscore

using BenchmarkDotNet.Attributes;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class QueryingBenchmarks
    {
        private const string AllTextPhrase = "__ALL";
        private const string AllNoneTextPhrase = "__ALL __NONE";

        private readonly string? topBrand;
        private readonly decimal? maxPrice;
        private readonly Index<Product> index;
        private readonly Index<Product> index_x100;

        public QueryingBenchmarks()
        {
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
                                new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, AllTextPhrase }),
                            }),
                        ProductMapper.Text),
                };

            this.index = IndexBuilder.Build(Product.Items, new ProductMapper(), synonymMapBindings);

            this.index_x100 = IndexBuilder.Build(Product.Items_x100, new ProductMapper(), synonymMapBindings);
        }

        [Benchmark]
        public void QueryComplex_x100()
        {
            using var result = this.index_x100.Query(
                this.index_x100.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, AllNoneTextPhrase)
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Rating, SortDirection.Descending));

            ConsumeResult(result);
        }

        [Benchmark]
        public void QueryComplex()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, AllNoneTextPhrase)
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price)
                    .Sort(ProductMapper.Rating, SortDirection.Descending));

            ConsumeResult(result);
        }

        [Benchmark]
        public void QuerySearch()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(ProductMapper.Text, SearchMode.Any, AllNoneTextPhrase));

            ConsumeResult(result);
        }

        [Benchmark]
        public void QueryFilter()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                    .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1));

            ConsumeResult(result);
        }

        [Benchmark]
        public void QueryFacet()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Facet(ProductMapper.Brand)
                    .Facet(ProductMapper.Category)
                    .Facet(ProductMapper.Price));

            ConsumeResult(result);
        }

        [Benchmark]
        public void QuerySort()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Sort(ProductMapper.Rating, SortDirection.Descending));

            ConsumeResult(result);
        }

        [Benchmark]
        public void Query()
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
