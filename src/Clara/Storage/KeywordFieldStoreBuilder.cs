using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStoreBuilder : FieldStoreBuilder
    {
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly Dictionary<int, HashSet<int>>? tokenDocuments;
        private readonly Dictionary<int, HashSet<int>>? documentTokens;

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

            var tokens = default(HashSet<int>);

            foreach (var token in tokenFieldValue.Keywords)
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                if (this.tokenDocuments is not null)
                {
#if NET6_0_OR_GREATER
                    ref var documents = ref CollectionsMarshal.GetValueRefOrAddDefault(this.tokenDocuments, tokenId, out _);

                    documents ??= new HashSet<int>();
#else
                    if (!this.tokenDocuments.TryGetValue(tokenId, out var documents))
                    {
                        this.tokenDocuments.Add(tokenId, documents = new HashSet<int>());
                    }
#endif

                    documents.Add(documentId);
                }

                if (this.documentTokens is not null)
                {
                    if (tokens == default)
                    {
                        tokens = new HashSet<int>();

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
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new KeywordDocumentTokenStore(tokenEncoder, this.documentTokens) : null);
        }
    }
}
