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
        private readonly double? maxPrice;
        private readonly Index<SampleProduct> index;

        public QueryBenchmarks()
        {
            this.allTextSynonym = Guid.NewGuid().ToString("N");

            this.topBrand = SampleProduct.Items
                .Select(x => x.Brand)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            this.maxPrice = SampleProduct.Items
                .Max(x => x.Price);

            var synonymMap =
                new SynonymMap(
                    SampleProductMapper.Text,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { SampleProductMapper.AllText, this.allTextSynonym }),
                    });

            var builder =
                new IndexBuilder<SampleProduct, SampleProduct>(
                    new SampleProductMapper(),
                    new[] { synonymMap });

            foreach (var item in SampleProduct.Items)
            {
                builder.Index(item);
            }

            this.index = builder.Build();
        }

        [Benchmark]
        public void SearchFilterFacetSortQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(SampleProductMapper.Text, this.allTextSynonym)
                    .Filter(SampleProductMapper.Brand, Values.Any(this.topBrand))
                    .Filter(SampleProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(SampleProductMapper.Brand)
                    .Facet(SampleProductMapper.Category)
                    .Facet(SampleProductMapper.Price)
                    .Sort(SampleProductMapper.Price, SortDirection.Descending));

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }

        [Benchmark]
        public void SearchFilterFacetQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Search(SampleProductMapper.Text, this.allTextSynonym)
                    .Filter(SampleProductMapper.Brand, Values.Any(this.topBrand))
                    .Filter(SampleProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(SampleProductMapper.Brand)
                    .Facet(SampleProductMapper.Category)
                    .Facet(SampleProductMapper.Price));

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }

        [Benchmark]
        public void FilterFacetSortQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Filter(SampleProductMapper.Brand, Values.Any(this.topBrand))
                    .Filter(SampleProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(SampleProductMapper.Brand)
                    .Facet(SampleProductMapper.Category)
                    .Facet(SampleProductMapper.Price)
                    .Sort(SampleProductMapper.Price, SortDirection.Descending));

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }

        [Benchmark]
        public void FilterFacetQuery()
        {
            using var result = this.index.Query(
                this.index.QueryBuilder()
                    .Filter(SampleProductMapper.Brand, Values.Any(this.topBrand))
                    .Filter(SampleProductMapper.Price, from: 1, to: this.maxPrice - 1)
                    .Facet(SampleProductMapper.Brand)
                    .Facet(SampleProductMapper.Category)
                    .Facet(SampleProductMapper.Price));

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }
    }
}
