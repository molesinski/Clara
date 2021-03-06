using System;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenEncoder : ITokenEncoder, IDisposable
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

        public void Dispose()
        {
            this.encoder.Dispose();
            this.decoder.Dispose();
        }
    }
}
