using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    public sealed class InstanceTokenEncoderStore : TokenEncoderStore
    {
        internal override ITokenEncoderBuilder CreateTokenEncoderBuilder()
        {
            return new TokenEncoderBuilder();
        }

        internal override ITokenEncoderBuilder CreateTokenEncoderBuilder(Field field)
        {
            return new TokenEncoderBuilder();
        }

        private sealed class TokenEncoderBuilder : ITokenEncoderBuilder
        {
            private readonly DictionarySlim<string, int> encoder = new();
            private readonly DictionarySlim<int, string> decoder = new();
            private int nextId = 1;
            private bool isBuilt;

            public int Encode(string token)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (this.isBuilt)
                {
                    throw new InvalidOperationException("Current instance is already built.");
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

            public ITokenEncoder Build()
            {
                if (this.isBuilt)
                {
                    throw new InvalidOperationException("Current instance is already built.");
                }

                var encoder = new TokenEncoder(this.encoder, this.decoder);

                this.isBuilt = true;

                return encoder;
            }

            private sealed class TokenEncoder : ITokenEncoder
            {
                private readonly DictionarySlim<string, int> encoder;
                private readonly DictionarySlim<int, string> decoder;

                public TokenEncoder(
                    DictionarySlim<string, int> encoder,
                    DictionarySlim<int, string> decoder)
                {
                    if (encoder is null)
                    {
                        throw new ArgumentNullException(nameof(encoder));
                    }

                    if (decoder is null)
                    {
                        throw new ArgumentNullException(nameof(decoder));
                    }

                    this.encoder = encoder;
                    this.decoder = decoder;
                }

                public bool TryEncode(string token, out int id)
                {
                    return this.encoder.TryGetValue(token, out id);
                }

                public string Decode(int id)
                {
                    if (!this.decoder.TryGetValue(id, out var token))
                    {
                        throw new InvalidOperationException("Specified id does not correspond to any encoded token.");
                    }

                    return token;
                }
            }
        }
    }
}
