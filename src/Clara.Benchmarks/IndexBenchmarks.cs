using BenchmarkDotNet.Attributes;
using Clara.Querying;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class IndexBenchmarks
    {
        private readonly Index<SampleProduct> index;
        private readonly Query query;

        public IndexBenchmarks()
        {
            var builder =
                new IndexBuilder<SampleProduct, SampleProduct>(
                    new SampleProductMapper());

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

            this.query = this.index.QueryBuilder()
                .Search(SampleProductMapper.Text, SampleProductMapper.AllText)
                .Filter(SampleProductMapper.Brand, Values.Any(brand))
                .Filter(SampleProductMapper.Price, from: 0, to: maxPrice - 1)
                .Facet(SampleProductMapper.Brand)
                .Facet(SampleProductMapper.Category)
                .Facet(SampleProductMapper.Price)
                .Sort(SampleProductMapper.Price, SortDirection.Descending)
                .Query;
        }

        [Benchmark]
        public void Query()
        {
            using var result = this.index.Query(this.query);

            foreach (var document in result.Documents)
            {
                _ = document;
            }
        }
    }
}
