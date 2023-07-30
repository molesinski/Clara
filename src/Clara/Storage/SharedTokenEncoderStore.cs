using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    public sealed class SharedTokenEncoderStore : TokenEncoderStore
    {
        private readonly TokenEncoderBuilder builder = new();
        private readonly Dictionary<Field, TokenEncoderBuilder> fieldBuilders = new();

        internal override ITokenEncoderBuilder CreateTokenEncoderBuilder()
        {
            return new TokenEncoderBuilderWrapper(this.builder);
        }

        internal override ITokenEncoderBuilder CreateTokenEncoderBuilder(Field field)
        {
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
        }

        private sealed class TokenEncoderBuilder : ITokenEncoderBuilder
        {
            private readonly DictionarySlim<string, int> encoder = new();
            private readonly DictionarySlim<int, string> decoder = new();
            private int nextId = 1;

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

            public ITokenEncoder Build()
            {
                var encoder = new DictionarySlim<string, int>(this.encoder);
                var decoder = new DictionarySlim<int, string>(this.decoder);

                return new TokenEncoder(encoder, decoder);
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
