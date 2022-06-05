using System;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly TextField<TSource> field;
        private readonly ISynonymMap synonymMap;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly PooledDictionarySlim<int, PooledHashSetSlim<int>> tokenDocuments;

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
            this.tokenDocuments = new();
        }

        public override void Index(int documentId, TSource item)
        {
            var text = this.field.ValueMapper(item);

            if (text is null)
            {
                return;
            }

            foreach (var token in this.synonymMap.GetTokens(text))
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                ref var documents = ref this.tokenDocuments.GetValueRefOrAddDefault(tokenId, out _);

                documents ??= new PooledHashSetSlim<int>();

                documents.Add(documentId);
            }
        }

        public override FieldStore Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();

            return
                new TextFieldStore(
                    this.synonymMap,
                    tokenEncoder,
                    new TokenDocumentStore(tokenEncoder, this.tokenDocuments));
        }
    }
}
