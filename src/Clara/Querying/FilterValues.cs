using Clara.Utils;

namespace Clara.Querying
{
    internal readonly struct FilterValues : IDisposable
    {
        private static readonly HashSetSlim<string> Empty = new();

        private readonly ObjectPoolLease<HashSetSlim<string>>? lease;

        public FilterValues(string? value)
        {
            this.lease = CreateLease(new StringEnumerable(value, trim: true));
        }

        public FilterValues(IEnumerable<string?>? values)
        {
            this.lease = CreateLease(new StringEnumerable(values, trim: true));
        }

        public readonly HashSetSlim<string> Value
        {
            get
            {
                return this.lease?.Instance ?? Empty;
            }
        }

        public void Dispose()
        {
            this.lease?.Dispose();
        }

        private static ObjectPoolLease<HashSetSlim<string>>? CreateLease(StringEnumerable values)
        {
            var lease = default(ObjectPoolLease<HashSetSlim<string>>?);

            foreach (var value in values)
            {
                lease ??= SharedObjectPools.FilterValues.Lease();
                lease.Value.Instance.Add(value);
            }

            return lease;
        }
    }
}
