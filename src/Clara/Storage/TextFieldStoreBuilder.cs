using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly TextField<TSource> field;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly ISynonymMap? synonymMap;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly DictionarySlim<int, float> documentLengths;
        private bool isBuilt;

        public TextFieldStoreBuilder(TextField<TSource> field, TokenEncoderBuilder tokenEncoderBuilder, ISynonymMap? synonymMap)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (tokenEncoderBuilder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoderBuilder));
            }

            this.field = field;
            this.tokenEncoderBuilder = tokenEncoderBuilder;
            this.synonymMap = synonymMap;
            this.tokenDocumentScores = new();
            this.documentLengths = new();
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var analyzer = this.synonymMap ?? this.field.Analyzer;

            foreach (var value in this.field.ValueMapper(item))
            {
                if (!string.IsNullOrWhiteSpace(value.Text))
                {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    foreach (var token in analyzer.GetTokens(value.Text))
#else
                    foreach (var token in analyzer.GetTokens(value.Text!))
#endif
                    {
                        var tokenId = this.tokenEncoderBuilder.Encode(token);

                        ref var documents = ref this.tokenDocumentScores.GetValueRefOrAddDefault(tokenId, out _);

                        documents ??= new DictionarySlim<int, float>();

                        ref var score = ref documents.GetValueRefOrAddDefault(documentId, out _);

                        score += value.Weight;

                        ref var length = ref this.documentLengths.GetValueRefOrAddDefault(documentId, out _);

                        length += value.Weight;
                    }
                }
            }
        }

        public override FieldStore Build(TokenEncoder tokenEncoder)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            var store =
                new TextFieldStore(
                    tokenEncoder,
                    this.field.Analyzer,
                    this.synonymMap,
                    new TextDocumentStore(tokenEncoder, this.tokenDocumentScores, this.documentLengths, this.field.Similarity));

            this.isBuilt = true;

            return store;
        }
    }
}
