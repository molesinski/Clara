using System;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly KeywordField<TSource> field;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>>? tokenDocuments;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>>? documentTokens;

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
            var values = this.field.ValueMapper(item);

            if (values is null)
            {
                return;
            }

            var tokens = default(PooledHashSetSlim<int>);

            foreach (var token in values)
            {
                if (token is null)
                {
                    continue;
                }

                var tokenId = this.tokenEncoderBuilder.Encode(token);

                if (this.tokenDocuments is not null)
                {
                    ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                    documents ??= new PooledHashSetSlim<int>();

                    documents.Add(documentId);
                }

                if (this.documentTokens is not null)
                {
                    if (tokens == default)
                    {
                        ref var value = ref this.documentTokens.GetValueRefOrAddDefault(documentId, out _);

                        value = tokens = new PooledHashSetSlim<int>();
                    }

                    tokens.Add(tokenId);
                }
            }
        }

        public override FieldStore Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();

            return
                new KeywordFieldStore(
                    tokenEncoder,
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new KeywordDocumentTokenStore(tokenEncoder, this.documentTokens) : null);
        }
    }
}
