using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class TextField : Field
    {
        internal TextField(IAnalyzer analyzer, Weight? weight = null)
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
            this.Weight = weight ?? Weight.Default;
        }

        public IAnalyzer Analyzer { get; }

        public Weight Weight { get; }
    }

    public sealed class TextField<TSource> : TextField
    {
        public TextField(Func<TSource, string?> valueMapper, IAnalyzer analyzer)
            : base(analyzer)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source));
        }

        public TextField(Func<TSource, IEnumerable<string?>?> valueMapper, IAnalyzer analyzer)
            : base(analyzer)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source));
        }

        internal Func<TSource, StringEnumerable> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new TextFieldStoreBuilder<TSource>(this, tokenEncoderStore, synonymMap);
        }
    }
}
