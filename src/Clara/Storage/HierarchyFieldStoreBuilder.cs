using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly PooledDictionary<string, IEnumerable<string>> hierarchyDecodeCache = new(Allocator.ArrayPool);
        private readonly HierarchyField<TSource> field;
        private readonly char separator;
        private readonly string root;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionary<int, PooledSet<int>>? parentChildren;
        private readonly PooledDictionary<int, PooledSet<int>>? tokenDocuments;
        private readonly PooledDictionary<int, PooledSet<int>>? documentTokens;
        private bool isBuilt;
        private bool isDisposed;

        public HierarchyFieldStoreBuilder(HierarchyField<TSource> field, TokenEncoderStore tokenEncoderStore)
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
            this.separator = field.Separator;
            this.root = field.Root;
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);

            if (field.IsFilterable)
            {
                this.tokenDocuments = new(Allocator.Mixed);
            }

            if (field.IsFacetable)
            {
                this.parentChildren = new(Allocator.Mixed);
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
            var tokens = default(PooledSet<int>);

            foreach (var hierarchyEncodedToken in values)
            {
                var decodedTokens = this.Decode(hierarchyEncodedToken);
                var parentId = -1;

                foreach (var token in decodedTokens)
                {
                    var tokenId = this.tokenEncoderBuilder.Encode(token);

                    if (parentId != -1)
                    {
                        if (this.parentChildren is not null)
                        {
                            ref var children = ref this.parentChildren.GetValueRefOrAddDefault(parentId, out _);

                            children ??= new PooledSet<int>(Allocator.Mixed);
                            children.Add(tokenId);
                        }
                    }

                    parentId = tokenId;

                    if (this.tokenDocuments is not null)
                    {
                        ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                        documents ??= new PooledSet<int>(Allocator.Mixed);
                        documents.Add(documentId);
                    }

                    if (this.documentTokens is not null)
                    {
                        if (tokens == default)
                        {
                            ref var value = ref this.documentTokens.GetValueRefOrAddDefault(documentId, out _);

                            value = tokens = new PooledSet<int>(Allocator.Mixed);
                        }

                        tokens.Add(tokenId);
                    }
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
                new HierarchyFieldStore(
                    tokenEncoder,
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null && this.parentChildren is not null ? new HierarchyDocumentTokenStore(this.root, tokenEncoder, this.documentTokens, this.parentChildren) : null);

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
                    this.parentChildren?.Dispose();
                }

                this.tokenEncoderBuilder.Dispose();
                this.hierarchyDecodeCache.Dispose();

                this.isDisposed = true;
            }
        }

        private IEnumerable<string> Decode(string hierarchyEncodedToken)
        {
            ref var decodedTokens = ref this.hierarchyDecodeCache.GetValueRefOrAddDefault(hierarchyEncodedToken, out _);

            if (decodedTokens is null)
            {
                var parts = hierarchyEncodedToken.Split(this.separator);
                var array = new string[1 + parts.Length];

                array[0] = this.root;

                for (var i = 0; i < parts.Length; i++)
                {
                    array[1 + i] = parts[i];
                }

                decodedTokens = array;
            }

            return decodedTokens;
        }
    }
}
