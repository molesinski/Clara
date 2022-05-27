using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Clara.Analysis
{
    public sealed class ObjectPool<TItem>
    {
        private readonly ConcurrentBag<TItem> objects = new();
        private readonly Func<TItem> factory;
        private readonly int maximumCount;
        private int count;

        public ObjectPool(Func<TItem> factory)
            : this(factory, maximumCount: Environment.ProcessorCount)
        {
        }

        public ObjectPool(Func<TItem> factory, int maximumCount)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (maximumCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumCount));
            }

            this.factory = factory;
            this.maximumCount = maximumCount;
        }

        public TItem Get()
        {
            if (this.objects.TryTake(out var item))
            {
                Interlocked.Decrement(ref this.count);
                return item;
            }

            return this.factory();
        }

        public void Return(TItem item)
        {
            if (this.count < this.maximumCount)
            {
                this.objects.Add(item);
                Interlocked.Increment(ref this.count);
            }
        }
    }
}
