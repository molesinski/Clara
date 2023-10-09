using Clara.Analysis;
using Clara.Utils;

namespace Clara.Storage
{
    internal abstract class TokenEncoderBuilder
    {
        private readonly StringPoolSlim pool = new();
        private readonly DictionarySlim<string, int> encoder = new();
        private readonly DictionarySlim<int, string> decoder = new();
        private int nextId = 1;

        public TokenEncoder Build()
        {
            return this.BuildCore(this.pool, this.encoder, this.decoder);
        }

        public int Encode(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            ref var id = ref this.encoder.GetValueRefOrAddDefault(value, out var exists);

            if (exists)
            {
                return id;
            }

            id = this.nextId++;
            this.decoder[id] = value;

            return id;
        }

        public int Encode(Token token)
        {
            var value = this.pool.GetOrAdd(token.AsReadOnlySpan());

            ref var id = ref this.encoder.GetValueRefOrAddDefault(value, out var exists);

            if (exists)
            {
                return id;
            }

            id = this.nextId++;
            this.decoder[id] = value;

            return id;
        }

        protected abstract TokenEncoder BuildCore(
            StringPoolSlim pool,
            DictionarySlim<string, int> encoder,
            DictionarySlim<int, string> decoder);
    }
}
