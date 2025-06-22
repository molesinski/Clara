using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentSet : IDisposable
    {
        private static readonly HashSetSlim<int> Empty = new();

        private readonly HashSetSlim<int>? value;
        private readonly ObjectPoolLease<HashSetSlim<int>>? lease;

        public DocumentSet(HashSetSlim<int> value)
        {
            this.value = value;
        }

        public DocumentSet(ObjectPoolLease<HashSetSlim<int>> lease)
        {
            this.lease = lease;
        }

        public readonly HashSetSlim<int> Value
        {
            get
            {
                return this.value ?? this.lease?.Instance ?? Empty;
            }
        }

        public void Dispose()
        {
            this.lease?.Dispose();
        }
    }
}
