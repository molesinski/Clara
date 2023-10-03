using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class HierarchyFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly DictionarySlim<string, string[]> hierarchyDecodeCache = new();
        private readonly HierarchyField<TSource> field;
        private readonly string[] separatorArray;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly DictionarySlim<int, HashSetSlim<int>>? parentChildren;
        private readonly DictionarySlim<int, HashSetSlim<int>>? tokenDocuments;
        private readonly DictionarySlim<int, HashSetSlim<int>>? documentTokens;
        private bool isBuilt;

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
            this.separatorArray = new[] { field.Separator };
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);

            if (field.IsFilterable)
            {
                this.tokenDocuments = new();
            }

            if (field.IsFacetable)
            {
                this.parentChildren = new();
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

                            children ??= new HashSetSlim<int>();
                            children.Add(tokenId);
                        }
                    }

                    parentId = tokenId;

                    if (this.tokenDocuments is not null)
                    {
                        ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                        documents ??= new HashSetSlim<int>();
                        documents.Add(documentId);
                    }

                    if (this.documentTokens is not null)
                    {
                        if (tokens == default)
                        {
                            ref var value = ref this.documentTokens.GetValueRefOrAddDefault(documentId, out _);

                            value = tokens = new HashSetSlim<int>();
                        }

                        tokens.Add(tokenId);
                    }
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
                new HierarchyFieldStore(
                    this.tokenDocuments is not null ? new TokenDocumentStore(tokenEncoder, this.tokenDocuments) : null,
                    this.documentTokens is not null && this.parentChildren is not null ? new HierarchyDocumentTokenStore(this.field, tokenEncoder, this.documentTokens, this.parentChildren) : null);

            this.isBuilt = true;

            return store;
        }

        private string[] Decode(string hierarchyEncodedToken)
        {
            ref var decodedTokens = ref this.hierarchyDecodeCache.GetValueRefOrAddDefault(hierarchyEncodedToken, out _);

            if (decodedTokens is null)
            {
                var parts = hierarchyEncodedToken.Split(this.separatorArray, StringSplitOptions.RemoveEmptyEntries);

                decodedTokens = new string[1 + parts.Length];
                decodedTokens[0] = this.field.Root;

                for (var i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Trim();

                    if (this.field.ValueHandling == HierarchyValueHandling.Path)
                    {
                        if (i == 0)
                        {
                            decodedTokens[1 + i] = parts[i];
                        }
                        else
                        {
                            decodedTokens[1 + i] = string.Concat(decodedTokens[1 + i - 1], this.field.Separator, parts[i]);
                        }
                    }
                    else
                    {
                        decodedTokens[1 + i] = parts[i];
                    }
                }
            }

            return decodedTokens;
        }
    }
}
