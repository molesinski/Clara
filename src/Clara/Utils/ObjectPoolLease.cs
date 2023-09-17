namespace Clara.Utils
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly struct ObjectPoolLease<TItem> : IDisposable
#pragma warning restore CA1815 // Override equals and operator equals on value types
        where TItem : class
    {
        private readonly ObjectPool<TItem> pool;
        private readonly TItem instance;

        internal ObjectPoolLease(
            ObjectPool<TItem> pool,
            TItem target)
        {
            this.pool = pool;
            this.instance = target;
        }

        public readonly TItem Instance
        {
            get
            {
                return this.instance;
            }
        }

        public readonly void Dispose()
        {
            this.pool.Return(this.instance);
        }
    }
}
