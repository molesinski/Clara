﻿using Clara.Analysis;
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

            public int Encode(Token token)
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
            private readonly DictionarySlim<Token, int> encoder = new();
            private readonly DictionarySlim<int, string> decoder = new();
            private int nextId = 1;

            public int Encode(Token token)
            {
                if (token.Length == 0)
                {
                    throw new ArgumentException("Token must be not empty.", nameof(token));
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
                var encoder = new DictionarySlim<Token, int>(this.encoder);
                var decoder = new DictionarySlim<int, string>(this.decoder);

                return new TokenEncoder(encoder, decoder);
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

                    return value;
                }
            }
        }
    }
}
