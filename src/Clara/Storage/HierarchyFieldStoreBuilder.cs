using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly HierarchyField<TSource> field;
        private readonly char separator;
        private readonly string root;
        private readonly IEnumerable<string> rootEnumerable;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>> parentChildren;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>>? tokenDocuments;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>>? documentTokens;

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
            this.rootEnumerable = new[] { field.Root };
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);
            this.parentChildren = new();

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

            foreach (var hierarchyEncodedToken in values)
            {
                if (hierarchyEncodedToken is null)
                {
                    continue;
                }

                var decodedTokens = hierarchyEncodedToken.Split(this.separator);
                var parentId = this.tokenEncoderBuilder.Encode(this.root);

                foreach (var token in this.rootEnumerable.Concat(decodedTokens))
                {
                    var tokenId = this.tokenEncoderBuilder.Encode(token);

                    ref var children = ref this.parentChildren.GetValueRefOrAddDefault(parentId, out _);

                    children ??= new PooledHashSetSlim<int>();

                    children.Add(tokenId);

                    parentId = tokenId;

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
                            tokens = new PooledHashSetSlim<int>();

                            this.documentTokens.Add(documentId, tokens);
                        }

                        tokens.Add(tokenId);
                    }
                }
            }
        }

        public override FieldStore Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();

            return
                new HierarchyFieldStore(
                    tokenEncoder,
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new HierarchyDocumentTokenStore(this.root, tokenEncoder, this.documentTokens, this.parentChildren) : null);
        }
    }
}
