using System;
using System.Collections.Generic;

namespace Clara.Storage
{
    internal sealed class TokenEncoder
    {
        private readonly Dictionary<string, int> encoder;
        private readonly Dictionary<int, string> decoder;

        public TokenEncoder(
            Dictionary<string, int> encoder,
            Dictionary<int, string> decoder)
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
