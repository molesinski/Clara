using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class TextField : Field
    {
        internal TextField(IAnalyzer analyzer, Similarity? similarity)
            : base(
                isFilterable: false,
                isFacetable: false,
                isSortable: false)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            this.Analyzer = analyzer;
            this.Similarity = similarity ?? Similarity.Default;
        }

        public IAnalyzer Analyzer { get; }

        public Similarity Similarity { get; }
    }

    public sealed class TextField<TSource> : TextField
    {
        public TextField(Func<TSource, string?> valueMapper, IAnalyzer analyzer, Similarity? similarity = null)
            : base(analyzer, similarity)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new PrimitiveEnumerable<TextWeight>(new TextWeight(valueMapper(source)));
        }

        public TextField(Func<TSource, IEnumerable<string?>?> valueMapper, IAnalyzer analyzer, Similarity? similarity = null)
            : base(analyzer, similarity)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new PrimitiveEnumerable<TextWeight>(valueMapper(source)?.Select(x => new TextWeight(x)));
        }

        public TextField(Func<TSource, IEnumerable<TextWeight>?> valueMapper, IAnalyzer analyzer, Similarity? similarity = null)
            : base(analyzer, similarity)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new PrimitiveEnumerable<TextWeight>(valueMapper(source));
        }

        public TextField(Func<TSource, IEnumerable<TextWeight?>?> valueMapper, IAnalyzer analyzer, Similarity? similarity = null)
            : base(analyzer, similarity)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new PrimitiveEnumerable<TextWeight>(valueMapper(source));
        }

        internal Func<TSource, PrimitiveEnumerable<TextWeight>> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new TextFieldStoreBuilder<TSource>(this, tokenEncoderStore, synonymMap);
        }
    }
}
