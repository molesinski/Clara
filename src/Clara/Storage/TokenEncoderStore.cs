using Clara.Mapping;

namespace Clara.Storage
{
    public abstract class TokenEncoderStore
    {
        protected internal TokenEncoderStore()
        {
        }

        public object SyncRoot { get; } = new object();

        internal abstract ITokenEncoderBuilder CreateTokenEncoderBuilder();

        internal abstract ITokenEncoderBuilder CreateTokenEncoderBuilder(Field field);
    }
}
