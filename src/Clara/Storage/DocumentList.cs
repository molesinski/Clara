using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentList : IDisposable
    {
        private static readonly ListSlim<int> Empty = new();

        private readonly ObjectPoolLease<ListSlim<int>>? lease;
        private readonly ListSlim<int>? instance;

        public DocumentList(ObjectPoolLease<ListSlim<int>> lease)
        {
            this.lease = lease;
            this.instance = null;
        }

        public DocumentList(ListSlim<int> instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.lease = null;
            this.instance = instance;
        }

        public readonly ListSlim<int> List
        {
            get
            {
                return this.lease?.Instance ?? this.instance ?? Empty;
            }
        }

        public void Dispose()
        {
            this.lease?.Dispose();
        }
    }
}
