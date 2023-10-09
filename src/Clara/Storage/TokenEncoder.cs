using Clara.Analysis;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenEncoder
    {
        private readonly StringPoolSlim pool;
        private readonly DictionarySlim<string, int> encoder;
        private readonly DictionarySlim<int, string> decoder;

        public TokenEncoder(
            StringPoolSlim pool,
            DictionarySlim<string, int> encoder,
            DictionarySlim<int, string> decoder)
        {
            this.pool = pool;
            this.encoder = encoder;
            this.decoder = decoder;
        }

        public string Decode(int id)
        {
            if (!this.decoder.TryGetValue(id, out var value))
            {
                throw new InvalidOperationException("Id does not map to any encoded value.");
            }

            return value;
        }

        public bool TryEncode(string value, out int id)
        {
            return this.encoder.TryGetValue(value, out id);
        }

        public Token? ToReadOnly(Token token)
        {
            if (this.pool.TryGet(token.AsReadOnlySpan(), out var value))
            {
                return new Token(value);
            }

            return null;
        }
    }
}
