#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1707 // Identifiers should not contain underscores

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
        private readonly SynonymMapBinding synonymMapBinding;

        public IndexBenchmarks()
        {
            this.sharedTokenEncoderStore = new SharedTokenEncoderStore();

            this.allTextSynonym = Guid.NewGuid().ToString("N");

            this.synonymMapBinding =
                new SynonymMapBinding(
                    new SynonymMap(
                        ProductMapper.Analyzer,
                        new Synonym[]
                        {
                            new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, this.allTextSynonym }),
                        }),
                    ProductMapper.Text);
        }

        [Benchmark]
        public void Index_x100()
        {
            _ = IndexBuilder.Build(Product.Items_x100, new ProductMapper());
        }

        [Benchmark]
        public void IndexSynonym()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper(), this.synonymMapBinding);
        }

        [Benchmark]
        public void Index()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper());
        }

        [Benchmark]
        public void SharedIndex_x100()
        {
            _ = IndexBuilder.Build(Product.Items_x100, new ProductMapper(), this.sharedTokenEncoderStore);
        }

        [Benchmark]
        public void SharedIndexSynonym()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper(), this.sharedTokenEncoderStore, this.synonymMapBinding);
        }

        [Benchmark]
        public void SharedIndex()
        {
            _ = IndexBuilder.Build(Product.Items, new ProductMapper(), this.sharedTokenEncoderStore);
        }
    }
}
