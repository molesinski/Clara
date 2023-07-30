using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly TextField<TSource> field;
        private readonly ISynonymMap synonymMap;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionary<int, PooledHashSet<int>> tokenDocuments;
        private bool isBuilt;
        private bool isDisposed;

        public TextFieldStoreBuilder(TextField<TSource> field, TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap)
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
            this.synonymMap = synonymMap ?? new SynonymMap(field, Array.Empty<Synonym>());
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);
            this.tokenDocuments = new(Allocator.Mixed);
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isDisposed || this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built or disposed.");
            }

            var text = this.field.ValueMapper(item);

            if (text is null)
            {
                return;
            }

            foreach (var token in this.synonymMap.GetTokens(text))
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

#pragma warning disable CA2000 // Dispose objects before losing scope
                documents ??= new PooledHashSet<int>(Allocator.Mixed);
#pragma warning restore CA2000 // Dispose objects before losing scope

                documents.Add(documentId);
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
                new TextFieldStore(
                    this.synonymMap,
                    tokenEncoder,
                    new TokenDocumentStore(tokenEncoder, this.tokenDocuments));

            this.isBuilt = true;

            return store;
        }

        public override void Dispose()
        {
            if (!this.isDisposed)
            {
                if (!this.isBuilt)
                {
                    this.tokenDocuments.Dispose();
                }

                this.tokenEncoderBuilder.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
