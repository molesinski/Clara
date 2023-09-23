#pragma warning disable CA1822 // Mark members as static

using BenchmarkDotNet.Attributes;
using Clara.Analysis.Synonyms;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class IndexBenchmarks
    {
        [Benchmark]
        public void Index()
        {
            var builder =
                new IndexBuilder<Product, Product>(
                    new ProductMapper());

            foreach (var item in Product.Items)
            {
                builder.Index(item);
            }

            _ = builder.Build();
        }

        [Benchmark]
        public void SynonymMapIndex()
        {
            var allTextSynonym = Guid.NewGuid().ToString("N");

            var synonymMap =
                new SynonymTree(
                    ProductMapper.Text,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, allTextSynonym }),
                    });

            var builder =
                new IndexBuilder<Product, Product>(
                    new ProductMapper(),
                    new[]
                    {
                        synonymMap,
                    });

            foreach (var item in Product.Items)
            {
                builder.Index(item);
            }

            _ = builder.Build();
        }
    }
}
