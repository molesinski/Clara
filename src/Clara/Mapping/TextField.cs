using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class TextField : Field
    {
        internal TextField(IAnalyzer analyzer, ISynonymMap? synonymMap, Similarity? similarity)
            : base(
                isFilterable: false,
                isFacetable: false,
                isSortable: false)
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
            this.Similarity = similarity;
        }

        public IAnalyzer Analyzer { get; }

        public ISynonymMap? SynonymMap { get; }

        public Similarity? Similarity { get; }
    }

    public sealed class TextField<TSource> : TextField
    {
        public TextField(Func<TSource, string?> valueMapper, IAnalyzer analyzer, ISynonymMap? synonymMap = null, Similarity? similarity = null)
            : base(analyzer, synonymMap, similarity)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source));
        }

        public TextField(Func<TSource, IEnumerable<string?>?> valueMapper, IAnalyzer analyzer, ISynonymMap? synonymMap = null, Similarity? similarity = null)
            : base(analyzer, synonymMap, similarity)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source));
        }

        internal Func<TSource, StringEnumerable> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder)
        {
            return new TextFieldStoreBuilder<TSource>(this, tokenEncoderBuilder);
        }
    }
}
