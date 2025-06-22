using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class KeywordField : Field
    {
        internal KeywordField(bool isFilterable, bool isFacetable)
        {
            if (!isFilterable && !isFacetable)
            {
                throw new InvalidOperationException("Filtering or faceting must be enabled.");
            }

            this.IsFilterable = isFilterable;
            this.IsFacetable = isFacetable;
        }

        public override bool IsFilterable { get; }

        public override bool IsFacetable { get; }
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

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder)
        {
            return new KeywordFieldStoreBuilder<TSource>(this, tokenEncoderBuilder);
        }
    }
}
