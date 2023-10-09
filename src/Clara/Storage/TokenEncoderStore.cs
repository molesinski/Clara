namespace Clara.Storage
{
    public abstract class TokenEncoderStore
    {
        internal TokenEncoderStore()
        {
        }

        public object SyncRoot { get; } = new object();

        internal abstract TokenEncoderBuilder CreateBuilder();
    }
}
