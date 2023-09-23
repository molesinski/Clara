using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class KeywordField : TokenField
    {
        internal KeywordField(bool isFilterable, bool isFacetable)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
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

            this.ValueMapper = source => new TokenValue(valueMapper(source));
        }

        public KeywordField(Func<TSource, IEnumerable<string>?> valueMapper, bool isFilterable = false, bool isFacetable = false)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = source => new TokenValue(valueMapper(source));
        }

        internal Func<TSource, TokenValue> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new KeywordFieldStoreBuilder<TSource>(this, tokenEncoderStore);
        }
    }
}
