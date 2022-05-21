using System;
using Clara.Collections;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStoreBuilder : FieldStoreBuilder
    {
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionary<int, PooledSet<int>>? tokenDocuments;
        private readonly PooledDictionary<int, PooledSet<int>>? documentTokens;

        public KeywordFieldStoreBuilder(KeywordField field, TokenEncoderStore tokenEncoderStore)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

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

        public override void Index(int documentId, FieldValue fieldValue)
        {
            if (fieldValue is not KeywordFieldValue tokenFieldValue)
            {
                throw new InvalidOperationException("Indexing of non token field values is not supported.");
            }

            var tokens = default(PooledSet<int>);

            foreach (var token in tokenFieldValue.Keywords)
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                if (this.tokenDocuments is not null)
                {
                    ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                    documents ??= new PooledSet<int>();

                    documents.Add(documentId);
                }

                if (this.documentTokens is not null)
                {
                    if (tokens == default)
                    {
                        tokens = new PooledSet<int>();

                        this.documentTokens.Add(documentId, tokens);
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
