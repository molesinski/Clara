using Clara.Utils;
using Xunit;

namespace Clara.Tests
{
    public class ObjectPoolSlimTests
    {
        [Fact]
        public async Task ConcurrentReturnsRetainAllInstancesWhenCapacityIsAvailable()
        {
            const int workerCount = 4;
            const int rounds = 1000;
            var created = 0;
            var reset = 0;
            var pool = new ObjectPoolSlim<object>(
                () =>
                {
                    Interlocked.Increment(ref created);
                    return new object();
                },
                _ => Interlocked.Increment(ref reset));
            var leases = Enumerable.Range(0, workerCount).Select(_ => pool.Lease()).ToArray();
            var original = leases.Select(lease => lease.Instance).ToArray();
            using var barrier = new Barrier(workerCount + 1);
            var workers = Enumerable.Range(0, workerCount).Select(worker => Task.Factory.StartNew(
                () =>
                {
                    for (var round = 0; round < rounds; round++)
                    {
                        Synchronize(barrier);
                        leases[worker].Dispose();
                        Synchronize(barrier);
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default)).ToArray();

            for (var round = 0; round < rounds; round++)
            {
                Synchronize(barrier);
                Synchronize(barrier);

                for (var i = 0; i < workerCount; i++)
                {
                    leases[i] = pool.Lease();
                }
            }

            await Task.WhenAll(workers);

            try
            {
                Assert.Equal(workerCount, created);
                Assert.Equal(workerCount * rounds, reset);

                foreach (var instance in original)
                {
                    Assert.Single(leases, lease => ReferenceEquals(lease.Instance, instance));
                }
            }
            finally
            {
                foreach (var lease in leases)
                {
                    lease.Dispose();
                }
            }
        }

        [Fact]
        public void ReturnedInstanceIsResetBeforeReuse()
        {
            var pool = new ObjectPoolSlim<ListSlim<int>>(() => new ListSlim<int>());
            var lease = pool.Lease();
            var instance = lease.Instance;
            instance.Add(42);
            lease.Dispose();

            using var next = pool.Lease();
            Assert.Same(instance, next.Instance);
            Assert.Empty(next.Instance);
        }

        private static void Synchronize(Barrier barrier)
        {
            if (!barrier.SignalAndWait(TimeSpan.FromSeconds(30)))
            {
                throw new TimeoutException("Concurrent pool test did not reach its next phase.");
            }
        }
    }
}
