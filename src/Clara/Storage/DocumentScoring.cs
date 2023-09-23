using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentScoring : IDisposable
    {
        private static readonly DictionarySlim<int, float> Empty = new();

        private readonly ObjectPoolLease<DictionarySlim<int, float>>? lease;
        private readonly DictionarySlim<int, float>? instance;

        public DocumentScoring(ObjectPoolLease<DictionarySlim<int, float>> lease)
        {
            this.lease = lease;
            this.instance = null;
        }

        public DocumentScoring(DictionarySlim<int, float> instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.lease = null;
            this.instance = instance;
        }

        public readonly DictionarySlim<int, float> Value
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
