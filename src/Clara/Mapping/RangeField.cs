﻿using Clara.Storage;
using Clara.Utils;

namespace Clara.Mapping
{
    public abstract class RangeField<TValue> : Field
        where TValue : struct, IComparable<TValue>
    {
        internal RangeField(TValue minValue, TValue maxValue, bool isFilterable, bool isFacetable, bool isSortable)
        {
            if (!(minValue.CompareTo(maxValue) <= 0))
            {
                throw new ArgumentException("Min value has to be less or equal to max value.", nameof(minValue));
            }

            if (!isFilterable && !isFacetable && !isSortable)
            {
                throw new InvalidOperationException("Filtering, faceting or sorting must be enabled.");
            }

            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.IsFilterable = isFilterable;
            this.IsFacetable = isFacetable;
            this.IsSortable = isSortable;
        }

        public TValue MinValue { get; }

        public TValue MaxValue { get; }

        public override bool IsFilterable { get; }

        public override bool IsFacetable { get; }

        public override bool IsSortable { get; }
    }

    public class RangeField<TSource, TValue> : RangeField<TValue>
        where TValue : struct, IComparable<TValue>
    {
        public RangeField(Func<TSource, TValue?> valueMapper, TValue minValue, TValue maxValue, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
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

            this.ValueMapper = source => new PrimitiveEnumerable<TValue>(valueMapper(source));
        }

        public RangeField(Func<TSource, IEnumerable<TValue>?> valueMapper, TValue minValue, TValue maxValue, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
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

            this.ValueMapper = source => new PrimitiveEnumerable<TValue>(valueMapper(source));
        }

        internal Func<TSource, PrimitiveEnumerable<TValue>> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder)
        {
            return new RangeFieldStoreBuilder<TSource, TValue>(this);
        }
    }
}
