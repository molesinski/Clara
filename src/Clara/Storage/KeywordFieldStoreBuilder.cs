using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly KeywordField<TSource> field;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly DictionarySlim<int, HashSetSlim<int>>? tokenDocuments;
        private readonly DictionarySlim<int, HashSetSlim<int>>? documentTokens;
        private bool isBuilt;

        public KeywordFieldStoreBuilder(KeywordField<TSource> field, TokenEncoderStore tokenEncoderStore)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            this.field = field;
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);

            if (field.IsFilterable)
            {
                this.tokenDocuments = new();
            }

            if (field.IsFacetable)
            {
                this.documentTokens = new();
            }
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var values = this.field.ValueMapper(item);
            var tokens = default(HashSetSlim<int>);

            foreach (var token in values)
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                if (this.tokenDocuments is not null)
                {
                    ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                    documents ??= new();
                    documents.Add(documentId);
                }

                if (this.documentTokens is not null)
                {
                    if (tokens == default)
                    {
                        ref var value = ref this.documentTokens.GetValueRefOrAddDefault(documentId, out _);

                        value = tokens = new();
                    }

                    tokens.Add(tokenId);
                }
            }
        }

        public override FieldStore Build()
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var tokenEncoder = this.tokenEncoderBuilder.Build();

            var store =
                new KeywordFieldStore(
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new KeywordDocumentTokenStore(tokenEncoder, this.documentTokens) : null);

            this.isBuilt = true;

            return store;
        }
    }
}
