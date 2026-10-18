namespace Clara.Utils
{
    public sealed class ObjectPoolSlim<TItem>
        where TItem : class
    {
        private const int DegreeOfParallelism = 5;
        private const int DefaultSizeFactor = 1;

        private readonly Item?[] items;
        private readonly Func<TItem> factory;
        private readonly Action<TItem> resetter;

        public ObjectPoolSlim(Func<TItem> factory)
            : this(factory, DefaultResetter, DefaultSizeFactor)
        {
        }

        public ObjectPoolSlim(Func<TItem> factory, int sizeFactor)
            : this(factory, DefaultResetter, sizeFactor)
        {
        }

        public ObjectPoolSlim(Func<TItem> factory, Action<TItem> resetter)
            : this(factory, resetter, DefaultSizeFactor)
        {
        }

        public ObjectPoolSlim(Func<TItem> factory, Action<TItem> resetter, int sizeFactor)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (resetter is null)
            {
                throw new ArgumentNullException(nameof(resetter));
            }

            if (sizeFactor < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeFactor));
            }

            this.items = new Item[Environment.ProcessorCount * DegreeOfParallelism * sizeFactor];
            this.factory = factory;
            this.resetter = resetter;
        }

        public ObjectPoolSlimLease<TItem> Lease()
        {
            for (var i = 0; i < this.items.Length; i++)
            {
                var item = this.items[i];

                if (item is not null)
                {
                    if (item == Interlocked.CompareExchange(ref this.items[i], null, item))
                    {
                        return new ObjectPoolSlimLease<TItem>(item);
                    }
                }
            }

            return new ObjectPoolSlimLease<TItem>(new Item(this, this.factory()));
        }

        private static void DefaultResetter(TItem instance)
        {
            (instance as IResettable)?.Reset();
        }

        private void Return(Item item)
        {
            this.resetter(item.Instance);

            for (var i = 0; i < this.items.Length; i++)
            {
                if (this.items[i] is null)
                {
                    if (Interlocked.CompareExchange(ref this.items[i], item, null) is null)
                    {
                        break;
                    }
                }
            }
        }

        internal sealed class Item
        {
            private readonly ObjectPoolSlim<TItem> pool;
            private long version;

            internal Item(ObjectPoolSlim<TItem> pool, TItem instance)
            {
                this.pool = pool;
                this.Instance = instance;
            }

            internal TItem Instance { get; }

            internal long NextLeaseVersion()
            {
                // Odd versions identify leases; the following even version closes the return claim.
                return Interlocked.Increment(ref this.version);
            }

            internal void Return(long leaseVersion)
            {
                // Close this lease before resetting or publishing, including when the resetter throws.
                if (Interlocked.CompareExchange(ref this.version, unchecked(leaseVersion + 1), leaseVersion) == leaseVersion)
                {
                    this.pool.Return(this);
                }
            }
        }
    }
}
