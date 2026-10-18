namespace Clara.Utils
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct ObjectPoolSlimLease<TItem> : IDisposable
        where TItem : class
    {
        private readonly ObjectPoolSlim<TItem>.Item? item;
        private readonly long version;

        internal ObjectPoolSlimLease(ObjectPoolSlim<TItem>.Item item)
        {
            this.item = item;
            this.version = item.NextLeaseVersion();
        }

        public readonly TItem Instance
        {
            get
            {
                return this.item is null ? default! : this.item.Instance;
            }
        }

        public readonly void Dispose()
        {
            this.item?.Return(this.version);
        }
    }
}
