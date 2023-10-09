using Clara.Utils;

namespace Clara.Storage
{
    public sealed class SharedTokenEncoderStore : TokenEncoderStore
    {
        private readonly SharedTokenEncoderBuilder builder = new();

        internal override TokenEncoderBuilder CreateBuilder()
        {
            return this.builder;
        }

        private sealed class SharedTokenEncoderBuilder : TokenEncoderBuilder
        {
            protected override TokenEncoder BuildCore(
                StringPoolSlim pool,
                DictionarySlim<string, int> encoder,
                DictionarySlim<int, string> decoder)
            {
                pool = new StringPoolSlim(pool);
                encoder = new DictionarySlim<string, int>(encoder);
                decoder = new DictionarySlim<int, string>(decoder);

                return new TokenEncoder(pool, encoder, decoder);
            }
        }
    }
}
