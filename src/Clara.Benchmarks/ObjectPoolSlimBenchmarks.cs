using BenchmarkDotNet.Attributes;
using Clara.Utils;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class ObjectPoolSlimBenchmarks
    {
        private readonly ObjectPoolSlim<object> pool = new(() => new object());

        [GlobalSetup]
        public void Setup()
        {
            this.pool.Lease().Dispose();
        }

        [Benchmark]
        public object LeaseAndReturn()
        {
            using var lease = this.pool.Lease();

            return lease.Instance;
        }
    }
}
