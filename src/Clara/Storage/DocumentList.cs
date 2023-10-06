using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentList : IDisposable
    {
        private static readonly ListSlim<int> Empty = new();

        private readonly ObjectPoolLease<ListSlim<int>>? lease;

        public DocumentList(ObjectPoolLease<ListSlim<int>> lease)
        {
            this.lease = lease;
        }

        public readonly ListSlim<int> Value
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
    }
}
