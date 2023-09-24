#pragma warning disable CA1822 // Mark members as static

using BenchmarkDotNet.Attributes;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class IndexBenchmarks
    {
        private readonly SharedTokenEncoderStore sharedTokenEncoderStore;
        private readonly string allTextSynonym;
        private readonly ISynonymMap synonymMap;

        public IndexBenchmarks()
        {
            this.sharedTokenEncoderStore = new SharedTokenEncoderStore();

            this.allTextSynonym = Guid.NewGuid().ToString("N");

            this.synonymMap =
                new SynonymTree(
                    ProductMapper.Text,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, this.allTextSynonym }),
                    });
        }

        [Benchmark]
        public void IndexX100()
        {
            _ = IndexBuilder.Build(Product.ItemsX100, new ProductMapper());
        }

        [Benchmark]
        public void IndexWithSynonymMap()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper(), this.synonymMap);
        }

        [Benchmark]
        public void Index()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper());
        }

        [Benchmark]
        public void SharedIndexX100()
        {
            _ = IndexBuilder.Build(Product.ItemsX100, new ProductMapper(), this.sharedTokenEncoderStore);
        }

        [Benchmark]
        public void SharedIndexWithSynonymMap()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper(), this.sharedTokenEncoderStore, this.synonymMap);
        }

        [Benchmark]
        public void SharedIndex()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper(), this.sharedTokenEncoderStore);
        }
    }
}
