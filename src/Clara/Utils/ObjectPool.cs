namespace Clara.Utils
{
    public sealed class ObjectPool<TItem>
        where TItem : class
    {
        private const int DegreeOfParallelism = 5;
        private const int DefaultSizeFactor = 1;

        private readonly Item[] items;
        private readonly Func<TItem> factory;
        private readonly Action<TItem> resetter;

        public ObjectPool(Func<TItem> factory)
            : this(factory, DefaultResetter, DefaultSizeFactor)
        {
        }

        public ObjectPool(Func<TItem> factory, int sizeFactor)
            : this(factory, DefaultResetter, sizeFactor)
        {
        }

        public ObjectPool(Func<TItem> factory, Action<TItem> resetter)
            : this(factory, resetter, DefaultSizeFactor)
        {
        }

        public ObjectPool(Func<TItem> factory, Action<TItem> resetter, int sizeFactor)
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

        public ObjectPoolLease<TItem> Lease()
        {
            var instance = default(TItem);

            for (var i = 0; i < this.items.Length; i++)
            {
                instance = this.items[i].Value;

                if (instance is not null)
                {
                    if (instance == Interlocked.CompareExchange(ref this.items[i].Value!, null, instance))
                    {
                        return new ObjectPoolLease<TItem>(this, instance);
                    }
                }
            }

            return new ObjectPoolLease<TItem>(this, this.factory());
        }

        internal void Return(TItem instance)
        {
            this.resetter(instance);

            for (var i = 0; i < this.items.Length; i++)
            {
                if (this.items[i].Value is null)
                {
                    this.items[i].Value = instance;
                    break;
                }
            }
        }

        private static void DefaultResetter(TItem instance)
        {
            (instance as IResettable)?.Reset();
        }

        private struct Item
        {
            public TItem Value;
        }
    }
}
