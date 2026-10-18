using Clara.Utils;
using Xunit;

namespace Clara.Tests
{
    public class ObjectPoolSlimLeaseTests
    {
        [Fact]
        public void DefaultLeaseCanBeDisposed()
        {
            var lease = default(ObjectPoolSlimLease<object>);

            lease.Dispose();
            lease.Dispose();
        }

#if NET8_0_OR_GREATER
        [Fact]
        public void RetainedLeasesDoNotAllocateAfterWarmup()
        {
            var created = 0;
            var pool = new ObjectPoolSlim<object>(() =>
            {
                created++;
                return new object();
            });

            for (var i = 0; i < 1000; i++)
            {
                pool.Lease().Dispose();
            }

            var before = GC.GetAllocatedBytesForCurrentThread();

            for (var i = 0; i < 100000; i++)
            {
                pool.Lease().Dispose();
            }

            var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            Assert.Equal(1, created);
            Assert.Equal(0, allocated);
        }
#endif
    }
}
