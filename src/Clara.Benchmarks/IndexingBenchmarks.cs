#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1822 // Mark members as static

using BenchmarkDotNet.Attributes;
using Clara.Storage;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class IndexingBenchmarks
    {
        [Benchmark]
        public void IndexInstance_x100()
        {
            IndexBuilder.Build(Product.Items_x100, new ProductMapper(), new InstanceTokenEncoderStore());
        }

        [Benchmark]
        public void IndexInstance()
        {
            IndexBuilder.Build(Product.Items, new ProductMapper(), new InstanceTokenEncoderStore());
        }

        [Benchmark]
        public void IndexShared_x100()
        {
            IndexBuilder.Build(Product.Items_x100, new ProductMapper(), SharedTokenEncoderStore.Default);
        }

        [Benchmark]
        public void IndexShared()
        {
            IndexBuilder.Build(Product.Items, new ProductMapper(), SharedTokenEncoderStore.Default);
        }
    }
}
