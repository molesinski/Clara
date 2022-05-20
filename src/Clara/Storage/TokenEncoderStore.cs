using Clara.Mapping;

namespace Clara.Storage
{
    public abstract class TokenEncoderStore
    {
        protected internal TokenEncoderStore()
        {
        }

        internal object SyncRoot { get; } = new object();

        internal abstract TokenEncoderBuilder CreateTokenEncoderBuilder();

        internal abstract TokenEncoderBuilder CreateTokenEncoderBuilder(Field field);
    }
}
