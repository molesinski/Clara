using System;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenEncoderBuilder
    {
        private readonly PooledDictionary<string, int> encoder = new();
        private readonly PooledDictionarySlim<int, string> decoder = new();
        private readonly bool copyOnBuild;
        private int nextId = 1;

        public TokenEncoderBuilder(bool copyOnBuild)
        {
            this.copyOnBuild = copyOnBuild;
        }

        public int Encode(string token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            ref var id = ref this.encoder.GetValueRefOrAddDefault(token, out var exists);

            if (!exists)
            {
                id = this.nextId++;

                ref var value = ref this.decoder.GetValueRefOrAddDefault(id, out _);

                value = token;
            }

            return id;
        }

        public TokenEncoder Build()
        {
            if (this.copyOnBuild)
            {
                var encoder = new PooledDictionary<string, int>(this.encoder);
                var decoder = new PooledDictionarySlim<int, string>(this.decoder);

                return new TokenEncoder(encoder, decoder);
            }

            return new TokenEncoder(this.encoder, this.decoder);
        }
    }
}
