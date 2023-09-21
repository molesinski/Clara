using BenchmarkDotNet.Attributes;
using Clara.Analysis.Synonyms;
using Clara.Querying;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class QueryBenchmarks
    {
        private readonly Index<SampleProduct> index;
        private readonly Query searchFilterFacetSortQuery;
        private readonly Query searchFilterFacetQuery;
        private readonly Query filterFacetSortQuery;
        private readonly Query filterFacetQuery;

        public QueryBenchmarks()
        {
            var allTextSynonym = Guid.NewGuid().ToString("N");

            var synonymMap =
                new SynonymMap(
                    SampleProductMapper.Text,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { SampleProductMapper.AllText, allTextSynonym }),
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

            var brand = SampleProduct.Items
                .Select(x => x.Brand)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            var maxPrice = SampleProduct.Items
                .Max(x => x.Price);

            this.searchFilterFacetSortQuery = this.index.QueryBuilder()
                .Search(SampleProductMapper.Text, allTextSynonym)
                .Filter(SampleProductMapper.Brand, Values.Any(brand))
                .Filter(SampleProductMapper.Price, from: 0, to: maxPrice - 1)
                .Facet(SampleProductMapper.Brand)
                .Facet(SampleProductMapper.Category)
                .Facet(SampleProductMapper.Price)
                .Sort(SampleProductMapper.Price, SortDirection.Descending)
                .Query;

            this.searchFilterFacetQuery = this.index.QueryBuilder()
                .Search(SampleProductMapper.Text, allTextSynonym)
                .Filter(SampleProductMapper.Brand, Values.Any(brand))
                .Filter(SampleProductMapper.Price, from: 0, to: maxPrice - 1)
                .Facet(SampleProductMapper.Brand)
                .Facet(SampleProductMapper.Category)
                .Facet(SampleProductMapper.Price)
                .Query;

            this.filterFacetSortQuery = this.index.QueryBuilder()
                .Filter(SampleProductMapper.Brand, Values.Any(brand))
                .Filter(SampleProductMapper.Price, from: 0, to: maxPrice - 1)
                .Facet(SampleProductMapper.Brand)
                .Facet(SampleProductMapper.Category)
                .Facet(SampleProductMapper.Price)
                .Sort(SampleProductMapper.Price, SortDirection.Descending)
                .Query;

            this.filterFacetQuery = this.index.QueryBuilder()
                .Filter(SampleProductMapper.Brand, Values.Any(brand))
                .Filter(SampleProductMapper.Price, from: 0, to: maxPrice - 1)
                .Facet(SampleProductMapper.Brand)
                .Facet(SampleProductMapper.Category)
                .Facet(SampleProductMapper.Price)
                .Query;
        }

        [Benchmark]
        public void SearchFilterFacetSort()
        {
            using var result = this.index.Query(this.searchFilterFacetSortQuery);

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }

        [Benchmark]
        public void SearchFilterFacet()
        {
            using var result = this.index.Query(this.searchFilterFacetQuery);

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }

        [Benchmark]
        public void FilterFacetSort()
        {
            using var result = this.index.Query(this.filterFacetSortQuery);

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }

        [Benchmark]
        public void FilterFacet()
        {
            using var result = this.index.Query(this.filterFacetQuery);

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }
    }
}
