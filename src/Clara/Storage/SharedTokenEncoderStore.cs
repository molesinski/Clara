using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    public sealed class SharedTokenEncoderStore : TokenEncoderStore, IDisposable
    {
        private readonly TokenEncoderBuilder builder = new();
        private readonly Dictionary<Field, TokenEncoderBuilder> fieldBuilders = new();
        private bool isDisposed;

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.builder.Dispose();

                foreach (var builder in this.fieldBuilders)
                {
                    builder.Value.Dispose();
                }

                this.isDisposed = true;
            }
        }

        internal override ITokenEncoderBuilder CreateTokenEncoderBuilder()
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Current instance is already disposed.");
            }

            return new TokenEncoderBuilderWrapper(this.builder);
        }

        internal override ITokenEncoderBuilder CreateTokenEncoderBuilder(Field field)
        {
            if (this.isDisposed)
            {
                throw new InvalidOperationException("Current instance is already disposed.");
            }

            if (!this.fieldBuilders.TryGetValue(field, out var builder))
            {
                this.fieldBuilders.Add(field, builder = new TokenEncoderBuilder());
            }

            return new TokenEncoderBuilderWrapper(builder);
        }

        private sealed class TokenEncoderBuilderWrapper : ITokenEncoderBuilder
        {
            private readonly ITokenEncoderBuilder tokenEncoderBuilder;

            public TokenEncoderBuilderWrapper(ITokenEncoderBuilder tokenEncoderBuilder)
            {
                this.tokenEncoderBuilder = tokenEncoderBuilder;
            }

            public int Encode(string token)
            {
                return this.tokenEncoderBuilder.Encode(token);
            }

            public ITokenEncoder Build()
            {
                return this.tokenEncoderBuilder.Build();
            }

            public void Dispose()
            {
            }
        }

        private sealed class TokenEncoderBuilder : ITokenEncoderBuilder
        {
            private readonly PooledDictionary<string, int> encoder = new(Allocator.Mixed);
            private readonly PooledDictionary<int, string> decoder = new(Allocator.Mixed);
            private int nextId = 1;
            private bool isDisposed;

            public int Encode(string token)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (this.isDisposed)
                {
                    throw new InvalidOperationException("Current instance is already disposed.");
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
                if (this.isDisposed)
                {
                    throw new InvalidOperationException("Current instance is already disposed.");
                }

                var encoder = new PooledDictionary<string, int>(Allocator.Mixed, this.encoder);
                var decoder = new PooledDictionary<int, string>(Allocator.Mixed, this.decoder);

                return new TokenEncoder(encoder, decoder);
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
