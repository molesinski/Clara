using System;
using Clara.Analysis.Synonyms;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class RangeField<TValue> : Field
        where TValue : struct, IComparable<TValue>
    {
        protected internal RangeField(TValue minValue, TValue maxValue, bool isFilterable, bool isFacetable, bool isSortable)
            : base(
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
            if (!isFilterable && !isFacetable && !isSortable)
            {
                throw new InvalidOperationException("Either filtering, faceting or sorting must be enabled for given field.");
            }

            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public TValue MinValue { get; }

        public TValue MaxValue { get; }
    }

    public class RangeField<TSource, TValue> : RangeField<TValue>
        where TValue : struct, IComparable<TValue>
    {
        public RangeField(Func<TSource, FieldValues<TValue>> valueMapper, TValue minValue, TValue maxValue, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
            : base(
                minValue: minValue,
                maxValue: maxValue,
                isFilterable: isFilterable,
                isFacetable: isFacetable,
                isSortable: isSortable)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = valueMapper;
        }

        public Func<TSource, FieldValues<TValue>> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(
            TokenEncoderStore tokenEncoderStore,
            ISynonymMap? synonymMap)
        {
            return new RangeFieldStoreBuilder<TSource, TValue>(this);
        }
    }
}
