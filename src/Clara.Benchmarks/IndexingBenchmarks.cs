#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1822 // Mark members as static

using BenchmarkDotNet.Attributes;
using Clara.Storage;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    [ShortRunJob]
    public class IndexingBenchmarks
    {
        private readonly SharedTokenEncoderStore sharedTokenEncoderStore;

        public IndexingBenchmarks()
        {
            this.sharedTokenEncoderStore = new SharedTokenEncoderStore();
        }

        [Benchmark]
        public void Index_x100()
        {
            IndexBuilder.Build(Product.Items_x100, new ProductMapper());
        }

        [Benchmark]
        public void Index()
        {
            IndexBuilder.Build(Product.Items, new ProductMapper());
        }

        [Benchmark]
        public void IndexShared_x100()
        {
            IndexBuilder.Build(Product.Items_x100, new ProductMapper(), this.sharedTokenEncoderStore);
        }

        [Benchmark]
        public void IndexShared()
        {
            IndexBuilder.Build(Product.Items, new ProductMapper(), this.sharedTokenEncoderStore);
        }
    }
}
