using Clara.Analysis;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly TextField<TSource> field;
        private readonly ITokenTermSource tokenTermSource;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly DictionarySlim<int, float> documentLengths;
        private bool isBuilt;

        public TextFieldStoreBuilder(TextField<TSource> field, TokenEncoderBuilder tokenEncoderBuilder)
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
            this.tokenTermSource = field.SynonymMap?.CreateIndexTokenTermSource() ?? field.Analyzer.CreateTokenTermSource();
            this.tokenEncoderBuilder = tokenEncoderBuilder;
            this.tokenDocumentScores = new();
            this.documentLengths = new();
        }

        public override void Index(int documentId, TSource item)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            foreach (var value in this.field.ValueMapper(item))
            {
                if (!string.IsNullOrWhiteSpace(value.Text))
                {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    var terms = this.tokenTermSource.GetTerms(value.Text);
#else
                    var terms = this.tokenTermSource.GetTerms(value.Text!);
#endif

                    foreach (var term in terms)
                    {
                        var tokenId = this.tokenEncoderBuilder.Encode(term.Token);

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

            (this.field.Similarity ?? Similarity.Default).Process(this.tokenDocumentScores, this.documentLengths);

            var store =
                new TextFieldStore(
                    new TextDocumentStore(tokenEncoder, this.field.Analyzer, this.field.SynonymMap, this.tokenDocumentScores));

            this.isBuilt = true;

            return store;
        }
    }
}
