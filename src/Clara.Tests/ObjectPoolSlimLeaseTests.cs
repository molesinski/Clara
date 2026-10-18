using Clara.Utils;
using Xunit;

namespace Clara.Tests
{
    public class ObjectPoolSlimLeaseTests
    {
        [Fact]
        public void RepeatedDisposeReturnsOnlyOnce()
        {
            var resets = 0;
            var pool = new ObjectPoolSlim<object>(() => new(), _ => resets++);
            var lease = pool.Lease();
            var instance = lease.Instance;

            lease.Dispose();
            lease.Dispose();

            Assert.Equal(1, resets);
            using var first = pool.Lease();
            using var second = pool.Lease();
            Assert.Same(instance, first.Instance);
            Assert.NotSame(first.Instance, second.Instance);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "Exercise disposal through a boxed copy of the lease.")]
        public void DisposingCopiesReturnsOnlyOnce()
        {
            var resets = 0;
            var pool = new ObjectPoolSlim<object>(() => new(), _ => resets++);
            var lease = pool.Lease();
            var copy = lease;
            IDisposable boxedCopy = lease;

            copy.Dispose();
            boxedCopy.Dispose();
            lease.Dispose();

            Assert.Equal(1, resets);
            using var first = pool.Lease();
            using var second = pool.Lease();
            Assert.Same(lease.Instance, first.Instance);
            Assert.NotSame(first.Instance, second.Instance);
        }

        [Fact]
        public void StaleCopyDoesNotReturnNewLease()
        {
            var resets = 0;
            var pool = new ObjectPoolSlim<List<int>>(
                () => new(),
                item =>
                {
                    resets++;
                    item.Clear();
                });
            var original = pool.Lease();
            var staleCopy = original;
            original.Dispose();

            for (var iteration = 0; iteration < 3; iteration++)
            {
                using var current = pool.Lease();
                Assert.Same(original.Instance, current.Instance);
                current.Instance.Add(iteration);

                staleCopy.Dispose();
                original.Dispose();

                Assert.Equal(iteration + 1, resets);
                Assert.Equal(iteration, Assert.Single(current.Instance));
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public async Task ConcurrentDisposalReturnsOnlyOnce(int workers)
        {
            var resets = 0;
            var pool = new ObjectPoolSlim<object>(() => new(), _ => Interlocked.Increment(ref resets));
            var lease = pool.Lease();
            using var barrier = new Barrier(workers);
            var tasks = Enumerable.Range(0, workers).Select(_ => Task.Factory.StartNew(
                () =>
                {
                    var copy = lease;

                    if (!barrier.SignalAndWait(TimeSpan.FromSeconds(10)))
                    {
                        throw new TimeoutException("Lease disposal synchronization timed out.");
                    }

                    copy.Dispose();
                },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default)).ToArray();

            await Task.WhenAll(tasks);
            lease.Dispose();

            Assert.Equal(1, resets);
            using var first = pool.Lease();
            using var second = pool.Lease();
            Assert.Same(lease.Instance, first.Instance);
            Assert.NotSame(first.Instance, second.Instance);
        }

        [Fact]
        public void DefaultLeaseCanBeDisposed()
        {
            var lease = default(ObjectPoolSlimLease<object>);

            lease.Dispose();
            lease.Dispose();
        }

        [Fact]
        public void ResetFailureDoesNotAllowAnotherReturn()
        {
            var resets = 0;
            var failure = new InvalidOperationException("Reset failed.");
            var pool = new ObjectPoolSlim<object>(
                () => new(),
                _ =>
                {
                    resets++;
                    throw failure;
                });
            var lease = pool.Lease();
            var copy = lease;

            Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => lease.Dispose()));
            copy.Dispose();
            lease.Dispose();

            Assert.Equal(1, resets);
            var next = pool.Lease();
            Assert.NotSame(lease.Instance, next.Instance);
            Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => next.Dispose()));
        }

        [Fact]
        public void DiscardedLeaseCannotBeReturnedAgainWhenASlotBecomesAvailable()
        {
            const int sizeFactor = 2;
            var capacity = Environment.ProcessorCount * 5 * sizeFactor;
            var resets = 0;
            var pool = new ObjectPoolSlim<object>(() => new(), _ => resets++, sizeFactor);
            var retained = Enumerable.Range(0, capacity).Select(_ => pool.Lease()).ToArray();
            var discarded = pool.Lease();
            var copy = discarded;

            foreach (var lease in retained)
            {
                lease.Dispose();
            }

            discarded.Dispose();
            using var first = pool.Lease();
            copy.Dispose();
            Assert.Equal(capacity + 1, resets);

            using var second = pool.Lease();
            Assert.Contains(retained, lease => ReferenceEquals(lease.Instance, first.Instance));
            Assert.Contains(retained, lease => ReferenceEquals(lease.Instance, second.Instance));
            Assert.NotSame(first.Instance, second.Instance);
            Assert.NotSame(discarded.Instance, second.Instance);
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
