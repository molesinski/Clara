using Clara.Utils;

namespace Clara.Storage
{
    public sealed class InstanceTokenEncoderStore : TokenEncoderStore
    {
        internal override TokenEncoderBuilder CreateBuilder()
        {
            return new InstanceTokenEncoderBuilder();
        }

        private sealed class InstanceTokenEncoderBuilder : TokenEncoderBuilder
        {
            protected override TokenEncoder BuildCore(
                StringPoolSlim pool,
                DictionarySlim<string, int> encoder,
                DictionarySlim<int, string> decoder)
            {
                return new TokenEncoder(pool, encoder, decoder);
            }
        }
    }
}
