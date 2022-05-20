using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Storage
{
    public sealed class SharedTokenEncoderStore : TokenEncoderStore
    {
        private readonly TokenEncoderBuilder builder = new(copyOnBuild: true);
        private readonly Dictionary<Field, TokenEncoderBuilder> fieldBuilders = new();

        internal override TokenEncoderBuilder CreateTokenEncoderBuilder()
        {
            return this.builder;
        }

        internal override TokenEncoderBuilder CreateTokenEncoderBuilder(Field field)
        {
            if (!this.fieldBuilders.TryGetValue(field, out var builder))
            {
                this.fieldBuilders.Add(field, builder = new TokenEncoderBuilder(copyOnBuild: true));
            }

            return builder;
        }
    }
}
