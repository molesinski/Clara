using Clara.Mapping;

namespace Clara.Storage
{
    public sealed class InstanceTokenEncoderStore : TokenEncoderStore
    {
        internal override TokenEncoderBuilder CreateTokenEncoderBuilder()
        {
            return new TokenEncoderBuilder(copyOnBuild: false);
        }

        internal override TokenEncoderBuilder CreateTokenEncoderBuilder(Field field)
        {
            return new TokenEncoderBuilder(copyOnBuild: false);
        }
    }
}
