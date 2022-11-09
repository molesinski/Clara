using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class TextField : TokenField
    {
        protected internal TextField(IAnalyzer analyzer)
            : base(
                isFilterable: true,
                isFacetable: false,
                isSortable: false)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            this.Analyzer = analyzer;
        }

        public IAnalyzer Analyzer { get; }
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

            this.ValueMapper = valueMapper;
        }

        public Func<TSource, string?> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new TextFieldStoreBuilder<TSource>(this, tokenEncoderStore, synonymMap);
        }
    }
}
