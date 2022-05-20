using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStoreBuilder : FieldStoreBuilder
    {
        private readonly char separator;
        private readonly string root;
        private readonly IEnumerable<string> rootEnumerable;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly Dictionary<int, HashSet<int>> parentChildren;
        private readonly Dictionary<int, HashSet<int>>? tokenDocuments;
        private readonly Dictionary<int, HashSet<int>>? documentTokens;

        public HierarchyFieldStoreBuilder(HierarchyField field, TokenEncoderStore tokenEncoderStore)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

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

        public override void Index(int documentId, FieldValue fieldValue)
        {
            if (fieldValue is not HierarchyFieldValue hierarchyFieldValue)
            {
                throw new InvalidOperationException("Indexing of non hierarchy field values is not supported.");
            }

            var tokens = default(HashSet<int>);

            foreach (var hierarchyEncodedToken in hierarchyFieldValue.Keywords)
            {
                var decodedTokens = hierarchyEncodedToken.Split(this.separator);
                var parentId = this.tokenEncoderBuilder.Encode(this.root);

                foreach (var token in this.rootEnumerable.Concat(decodedTokens))
                {
                    var tokenId = this.tokenEncoderBuilder.Encode(token);

#if NET6_0_OR_GREATER
                    ref var children = ref CollectionsMarshal.GetValueRefOrAddDefault(this.parentChildren, parentId, out _);

                    children ??= new HashSet<int>();
#else
                    if (!this.parentChildren.TryGetValue(parentId, out var children))
                    {
                        this.parentChildren.Add(parentId, children = new HashSet<int>());
                    }
#endif

                    children.Add(tokenId);

                    parentId = tokenId;

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
        }

        public override FieldStore Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();

            return
                new HierarchyFieldStore(
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null ? new HierarchyDocumentTokenStore(this.root, tokenEncoder, this.documentTokens, this.parentChildren) : null);
        }
    }
}
