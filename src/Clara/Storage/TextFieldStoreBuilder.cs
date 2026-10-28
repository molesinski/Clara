using Clara.Analysis;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextFieldStoreBuilder<TSource> : FieldStoreBuilder<TSource>
    {
        private readonly TextField<TSource> field;
        private readonly ITokenTermSource analyzerTermSource;
        private readonly ITokenTermSource synonymTermSource;
        private readonly TokenEncoderBuilder tokenEncoderBuilder;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;
        private readonly DictionarySlim<int, float> documentLengths;
        private readonly ITokenTermSource? surfaceTermSource;
        private readonly StringPoolSlim? surfacePool;
        private readonly DictionarySlim<string, int>? surfaceTokens;
        private readonly DictionarySlim<int, int>? positionTokens;
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
            this.analyzerTermSource = field.Analyzer.CreateTokenTermSource();
            this.synonymTermSource = field.SynonymMap?.CreateTokenTermSource() ?? this.analyzerTermSource;
            this.tokenEncoderBuilder = tokenEncoderBuilder;
            this.tokenDocumentScores = new();
            this.documentLengths = new();

            if (field.TokenExpansion is not null)
            {
                this.surfaceTermSource = field.TokenExpansion.Analyzer.CreateTokenTermSource();
                this.surfacePool = new();
                this.surfaceTokens = new();
                this.positionTokens = new();
            }
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
                    var terms =
                        value.ExpandSynonyms
                            ? this.synonymTermSource.GetTerms(value.Text)
                            : this.analyzerTermSource.GetTerms(value.Text);
#else
                    var terms =
                        value.ExpandSynonyms
                            ? this.synonymTermSource.GetTerms(value.Text!)
                            : this.analyzerTermSource.GetTerms(value.Text!);
#endif

                    foreach (var term in terms)
                    {
                        var tokenId = this.tokenEncoderBuilder.Encode(term.Token);

                        ref var documents = ref this.tokenDocumentScores.GetValueRefOrAddDefault(tokenId, out _);

                        documents ??= new DictionarySlim<int, float>();

                        ref var score = ref documents.GetValueRefOrAddDefault(documentId, out _);

                        score = this.field.IndexScoreAggregation.Combine(score, value.Weight);

                        ref var length = ref this.documentLengths.GetValueRefOrAddDefault(documentId, out _);

                        length = this.field.IndexScoreAggregation.Combine(length, value.Weight);
                    }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    this.IndexSurfaceTokens(value.Text);
#else
                    this.IndexSurfaceTokens(value.Text!);
#endif
                }
            }
        }

        public override FieldStore Build(TokenEncoder tokenEncoder)
        {
            if (this.isBuilt)
            {
                throw new InvalidOperationException("Current instance is already built.");
            }

            this.field.Similarity.Transform(this.tokenDocumentScores, this.documentLengths);

            var tokenExpansionStore = default(TokenExpansionStore);

            if (this.field.TokenExpansion is not null && this.surfaceTokens is not null)
            {
                var surfaceTokens = new DictionarySlim<string, int>();

                foreach (var pair in this.surfaceTokens)
                {
                    if (this.tokenDocumentScores.ContainsKey(pair.Value))
                    {
                        ref var tokenId = ref surfaceTokens.GetValueRefOrAddDefault(pair.Key, out _);

                        tokenId = pair.Value;
                    }
                }

                tokenExpansionStore = new TokenExpansionStore(this.field.TokenExpansion, surfaceTokens);
            }

            var store =
                new TextFieldStore(
                    new TextDocumentStore(tokenEncoder, this.field.Analyzer, this.field.SynonymMap, this.field.SearchScoreAggregation, this.tokenDocumentScores, tokenExpansionStore));

            this.isBuilt = true;

            return store;
        }

        private void IndexSurfaceTokens(string text)
        {
            if (this.surfaceTermSource is null || this.surfacePool is null || this.surfaceTokens is null || this.positionTokens is null)
            {
                return;
            }

            this.positionTokens.Clear();

            foreach (var term in this.analyzerTermSource.GetTerms(text))
            {
                ref var tokenId = ref this.positionTokens.GetValueRefOrAddDefault(term.Position.Start, out _);

                tokenId = this.tokenEncoderBuilder.Encode(term.Token);
            }

            foreach (var term in this.surfaceTermSource.GetTerms(text))
            {
                if (this.positionTokens.TryGetValue(term.Position.Start, out var tokenId))
                {
                    var surface = this.surfacePool.GetOrAdd(term.Token.AsReadOnlySpan());

                    ref var surfaceTokenId = ref this.surfaceTokens.GetValueRefOrAddDefault(surface, out var exists);

                    if (!exists)
                    {
                        surfaceTokenId = tokenId;
                    }
                }
            }
        }
    }
}
