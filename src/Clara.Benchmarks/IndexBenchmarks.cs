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
                new IndexBuilder<SampleProduct, SampleProduct>(
                    new SampleProductMapper());

            foreach (var item in SampleProduct.Items)
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

            _ = builder.Build();
        }
    }
}
