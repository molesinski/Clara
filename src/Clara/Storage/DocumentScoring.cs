using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentScoring : IDisposable
    {
        private static readonly DictionarySlim<int, float> Empty = new();

        private readonly ObjectPoolLease<DictionarySlim<int, float>>? lease;

        public DocumentScoring(ObjectPoolLease<DictionarySlim<int, float>> lease)
        {
            this.lease = lease;
        }

        public readonly DictionarySlim<int, float> Value
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
