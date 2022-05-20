using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder : FieldStoreBuilder
    {
        private readonly ITokenizer tokenizer;
        private readonly ISynonymMap synonymMap;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly Dictionary<int, HashSet<int>> tokenDocuments;

        public TextFieldStoreBuilder(TextField field, TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (tokenEncoderStore is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderStore));
            }

            this.tokenizer = field.Tokenizer;
            this.synonymMap = synonymMap ?? new SynonymMap(field, Array.Empty<Synonym>());
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);
            this.tokenDocuments = new();
        }

        public override void Index(int documentId, FieldValue fieldValue)
        {
            if (fieldValue is not TextFieldValue textFieldValue)
            {
                throw new InvalidOperationException("Indexing of non text field values is not supported.");
            }

            foreach (var token in this.synonymMap.Filter(this.tokenizer.GetTokens(textFieldValue.Text)))
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

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
        }

        public override FieldStore Build()
        {
            var tokenEncoder = this.tokenEncoderBuilder.Build();

            return
                new TextFieldStore(
                    this.synonymMap,
                    new TokenDocumentStore(tokenEncoder, this.tokenDocuments));
        }
    }
}
