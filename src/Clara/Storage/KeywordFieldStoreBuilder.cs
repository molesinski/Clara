using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class KeywordFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly KeywordField<TSource> field;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionary<int, PooledHashSet<int>>? tokenDocuments;
        private readonly PooledDictionary<int, PooledHashSet<int>>? documentTokens;
        private bool isBuilt;
        private bool isDisposed;

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
                this.tokenDocuments = new(Allocator.Mixed);
            }

            if (field.IsFacetable)
            {
                this.documentTokens = new(Allocator.Mixed);
            }
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isDisposed || this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built or disposed.");
            }

            var values = this.field.ValueMapper(item);
            var tokens = default(PooledHashSet<int>);

            foreach (var token in values)
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                if (this.tokenDocuments is not null)
                {
                    ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

#pragma warning disable CA2000 // Dispose objects before losing scope
                    documents ??= new PooledHashSet<int>(Allocator.Mixed);
#pragma warning restore CA2000 // Dispose objects before losing scope

                    documents.Add(documentId);
                }

                if (this.documentTokens is not null)
                {
                    if (tokens == default)
                    {
                        ref var value = ref this.documentTokens.GetValueRefOrAddDefault(documentId, out _);

#pragma warning disable CA2000 // Dispose objects before losing scope
                        value = tokens = new PooledHashSet<int>(Allocator.Mixed);
#pragma warning restore CA2000 // Dispose objects before losing scope
                    }

                    tokens.Add(tokenId);
                }
            }
        }

        public override FieldStore Build()
        {
            if (this.isDisposed || this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built or disposed.");
            }

            var tokenEncoder = this.tokenEncoderBuilder.Build();

            var store =
                new KeywordFieldStore(
                    tokenEncoder,
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new KeywordDocumentTokenStore(tokenEncoder, this.documentTokens) : null);

            this.isBuilt = true;

            return store;
        }

        public override void Dispose()
        {
            if (!this.isDisposed)
            {
                if (!this.isBuilt)
                {
                    this.tokenDocuments?.Dispose();
                    this.documentTokens?.Dispose();
                }

                this.tokenEncoderBuilder.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
