namespace Clara.Utils
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct ObjectPoolSlimLease<TItem> : IDisposable
        where TItem : class
    {
        private readonly ObjectPoolSlim<TItem> pool;
        private readonly TItem instance;

        internal ObjectPoolSlimLease(
            ObjectPoolSlim<TItem> pool,
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
