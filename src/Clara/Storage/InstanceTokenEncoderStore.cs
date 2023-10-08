using Clara.Analysis;
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
            private readonly DictionarySlim<Token, int> encoder = new();
            private readonly DictionarySlim<int, string> decoder = new();
            private int nextId = 1;
            private bool isBuilt;

            public int Encode(Token token)
            {
                if (token.IsEmpty)
                {
                    throw new ArgumentException("Token must be not empty.", nameof(token));
                }

                if (this.isBuilt)
                {
                    throw new InvalidOperationException("Current instance is already built.");
                }

                if (this.encoder.TryGetValue(token, out var id))
                {
                    return id;
                }

                var value = token.ToString();

                id = this.nextId++;
                this.encoder[new Token(value)] = id;
                this.decoder[id] = value;

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
                private readonly DictionarySlim<Token, int> encoder;
                private readonly DictionarySlim<int, string> decoder;

                public TokenEncoder(
                    DictionarySlim<Token, int> encoder,
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

                public bool TryEncode(Token token, out int id)
                {
                    return this.encoder.TryGetValue(token, out id);
                }

                public Token? ToReadOnly(Token token)
                {
                    if (this.encoder.TryGetValue(token, out var id))
                    {
                        return new Token(this.decoder[id]);
                    }

                    return null;
                }

                public string Decode(int id)
                {
                    if (!this.decoder.TryGetValue(id, out var value))
                    {
                        throw new InvalidOperationException("Specified id does not correspond to any encoded token.");
                    }

                    return value.ToString();
                }
            }
        }
    }
}
