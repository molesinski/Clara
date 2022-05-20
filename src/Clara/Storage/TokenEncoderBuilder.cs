using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Clara.Storage
{
    internal sealed class TokenEncoderBuilder
    {
        private readonly Dictionary<string, int> encoder = new();
        private readonly Dictionary<int, string> decoder = new();
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

#if NET6_0_OR_GREATER
            ref var id = ref CollectionsMarshal.GetValueRefOrAddDefault(this.encoder, token, out var exists);

            if (!exists)
            {
                id = this.nextId++;
                this.decoder.Add(id, token);
            }
#else
            if (!this.encoder.TryGetValue(token, out var id))
            {
                id = this.nextId++;
                this.encoder.Add(token, id);
                this.decoder.Add(id, token);
            }
#endif

            return id;
        }

        public TokenEncoder Build()
        {
            if (this.copyOnBuild)
            {
                var encoder = new Dictionary<string, int>(this.encoder);
                var decoder = new Dictionary<int, string>(this.decoder);

                return new TokenEncoder(encoder, decoder);
            }

            return new TokenEncoder(this.encoder, this.decoder);
        }
    }
}
