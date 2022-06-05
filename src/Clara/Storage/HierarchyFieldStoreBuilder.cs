using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly DictionarySlim<string, IEnumerable<string>> hierarchyDecodeCache = new(Allocator.ArrayPool);
        private readonly HierarchyField<TSource> field;
        private readonly char separator;
        private readonly string root;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly DictionarySlim<int, HashSetSlim<int>> parentChildren;
        private readonly DictionarySlim<int, HashSetSlim<int>>? tokenDocuments;
        private readonly DictionarySlim<int, HashSetSlim<int>>? documentTokens;

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
            this.parentChildren = new(Allocator.Mixed);

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
            var values = this.field.ValueMapper(item);
            var tokens = default(HashSetSlim<int>);

            foreach (var hierarchyEncodedToken in values)
            {
                var decodedTokens = this.Decode(hierarchyEncodedToken);
                var parentId = -1;

                foreach (var token in decodedTokens)
                {
                    var tokenId = this.tokenEncoderBuilder.Encode(token);

                    if (parentId != -1)
                    {
                        ref var children = ref this.parentChildren.GetValueRefOrAddDefault(parentId, out _);

                        children ??= new HashSetSlim<int>(Allocator.Mixed);
                        children.Add(tokenId);
                    }

                    parentId = tokenId;

                    if (this.tokenDocuments is not null)
                    {
                        ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                        documents ??= new HashSetSlim<int>(Allocator.Mixed);
                        documents.Add(documentId);
                    }

                    if (this.documentTokens is not null)
                    {
                        if (tokens == default)
                        {
                            ref var value = ref this.documentTokens.GetValueRefOrAddDefault(documentId, out _);

                            value = tokens = new HashSetSlim<int>(Allocator.Mixed);
                        }

                        tokens.Add(tokenId);
                    }
                }
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

        public override FieldStore Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();

            return
                new HierarchyFieldStore(
                    tokenEncoder,
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new HierarchyDocumentTokenStore(this.root, tokenEncoder, this.documentTokens, this.parentChildren) : null);
        }

        public override void Dispose()
        {
            this.hierarchyDecodeCache.Dispose();
        }
    }
}
