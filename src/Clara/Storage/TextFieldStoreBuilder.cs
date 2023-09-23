using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly TextField<TSource> field;
        private readonly ISynonymMap? synonymMap;
        private readonly ITokenEncoderBuilder tokenEncoderBuilder;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly DictionarySlim<int, int> documentLengths;
        private bool isBuilt;

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
            this.synonymMap = synonymMap;
            this.tokenEncoderBuilder = tokenEncoderStore.CreateTokenEncoderBuilder(field);
            this.tokenDocumentScores = new();
            this.documentLengths = new();
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var text = this.field.ValueMapper(item);

            if (text is null)
            {
                return;
            }

            var analyzer = this.synonymMap ?? this.field.Analyzer;

            foreach (var token in analyzer.GetTokens(text))
            {
                var tokenId = this.tokenEncoderBuilder.Encode(token);

                ref var documents = ref this.tokenDocumentScores.GetValueRefOrAddDefault(tokenId, out _);

                documents ??= new DictionarySlim<int, float>();

                ref var score = ref documents.GetValueRefOrAddDefault(documentId, out _);

                score++;

                ref var length = ref this.documentLengths.GetValueRefOrAddDefault(documentId, out _);

                length++;
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
                new TextFieldStore(
                    this.field.Analyzer,
                    this.synonymMap,
                    new TextDocumentStore(tokenEncoder, this.tokenDocumentScores, this.documentLengths, this.field.Weight));

            this.isBuilt = true;

            return store;
        }
    }
}
