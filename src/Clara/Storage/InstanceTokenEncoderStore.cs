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
            private readonly PooledDictionary<string, int> encoder = new(Allocator.Mixed);
            private readonly PooledDictionary<int, string> decoder = new(Allocator.Mixed);
            private int nextId = 1;
            private bool isBuilt;
            private bool isDisposed;

            public int Encode(string token)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (this.isDisposed || this.isBuilt)
                {
                    throw new InvalidOperationException("Current instance is already built or disposed.");
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
                if (this.isDisposed || this.isBuilt)
                {
                    throw new InvalidOperationException("Current instance is already built or disposed.");
                }

                var encoder = new TokenEncoder(this.encoder, this.decoder);

                this.isBuilt = true;

                return encoder;
            }

            public void Dispose()
            {
                if (!this.isDisposed)
                {
                    if (!this.isBuilt)
                    {
                        this.encoder.Dispose();
                        this.decoder.Dispose();
                    }

                    this.isDisposed = true;
                }
            }

            private sealed class TokenEncoder : ITokenEncoder
            {
                private readonly PooledDictionary<string, int> encoder;
                private readonly PooledDictionary<int, string> decoder;
                private bool isDisposed;

                public TokenEncoder(
                    PooledDictionary<string, int> encoder,
                    PooledDictionary<int, string> decoder)
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
                    if (this.isDisposed)
                    {
                        throw new InvalidOperationException("Current instance is already disposed.");
                    }

                    return this.encoder.TryGetValue(token, out id);
                }

                public string Decode(int id)
                {
                    if (this.isDisposed)
                    {
                        throw new InvalidOperationException("Current instance is already disposed.");
                    }

                    if (!this.decoder.TryGetValue(id, out var token))
                    {
                        throw new InvalidOperationException("Specified id does not correspond to any encoded token.");
                    }

                    return token;
                }

                public void Dispose()
                {
                    if (!this.isDisposed)
                    {
                        this.encoder.Dispose();
                        this.decoder.Dispose();

                        this.isDisposed = true;
                    }
                }
            }
        }
    }
}
