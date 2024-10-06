namespace Clara.Utils
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct ObjectPoolLease<TItem> : IDisposable
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
