using Clara.Analysis;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class TextField : Field
    {
        internal TextField(IAnalyzer analyzer, ISynonymMap? synonymMap, Similarity? similarity, ScoreAggregation? scoreAggregation)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (synonymMap is not null)
            {
                if (!ReferenceEquals(analyzer, synonymMap.Analyzer))
                {
                    throw new ArgumentException("Synonym map must use same analyzer instance as text field.", nameof(synonymMap));
                }
            }

            this.Analyzer = analyzer;
            this.SynonymMap = synonymMap;
            this.Similarity = similarity ?? Similarity.Default;
            this.ScoreAggregation = scoreAggregation ?? ScoreAggregation.Default;
        }

        public IAnalyzer Analyzer { get; }

        public ISynonymMap? SynonymMap { get; }

        public Similarity Similarity { get; }

        public ScoreAggregation ScoreAggregation { get; }

        public override bool IsSearchable
        {
            get
            {
                return true;
            }
        }
    }

    public sealed class TextField<TSource> : TextField
    {
        public TextField(Func<TSource, string?> valueMapper, IAnalyzer analyzer, ISynonymMap? synonymMap = null, Similarity? similarity = null, ScoreAggregation? scoreAggregation = null)
            : base(analyzer, synonymMap, similarity, scoreAggregation)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new TextWeightEnumerable(valueMapper(source));
        }

        public TextField(Func<TSource, IEnumerable<string?>?> valueMapper, IAnalyzer analyzer, ISynonymMap? synonymMap = null, Similarity? similarity = null, ScoreAggregation? scoreAggregation = null)
            : base(analyzer, synonymMap, similarity, scoreAggregation)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new TextWeightEnumerable(valueMapper(source));
        }

        public TextField(Func<TSource, IEnumerable<TextWeight>?> valueMapper, IAnalyzer analyzer, ISynonymMap? synonymMap = null, Similarity? similarity = null, ScoreAggregation? scoreAggregation = null)
            : base(analyzer, synonymMap, similarity, scoreAggregation)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new TextWeightEnumerable(valueMapper(source));
        }

        internal Func<TSource, TextWeightEnumerable> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder)
        {
            return new TextFieldStoreBuilder<TSource>(this, tokenEncoderBuilder);
        }
    }
}
