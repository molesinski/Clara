using Clara.Analysis.Synonyms;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class KeywordField : Field
    {
        internal KeywordField(bool isFilterable, bool isFacetable)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: false)
        {
            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Either filtering or faceting must be enabled for given field.");
            }
        }
    }

    public sealed class KeywordField<TSource> : KeywordField
    {
        public KeywordField(Func<TSource, string?> valueMapper, bool isFilterable = false, bool isFacetable = false)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source), trim: true);
        }

        public KeywordField(Func<TSource, IEnumerable<string?>?> valueMapper, bool isFilterable = false, bool isFacetable = false)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new StringEnumerable(valueMapper(source), trim: true);
        }

        internal Func<TSource, StringEnumerable> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new KeywordFieldStoreBuilder<TSource>(this, tokenEncoderStore);
        }
    }
}
