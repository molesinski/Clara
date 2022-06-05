using System;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenEncoderBuilder
    {
        private readonly DictionarySlim<string, int> encoder = new(Allocator.Default);
        private readonly DictionarySlim<int, string> decoder = new(Allocator.Default);
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
                var encoder = new DictionarySlim<string, int>(Allocator.Default, this.encoder);
                var decoder = new DictionarySlim<int, string>(Allocator.Default, this.decoder);

                return new TokenEncoder(encoder, decoder);
            }

            return new TokenEncoder(this.encoder, this.decoder);
        }
    }
}
