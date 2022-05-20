using System;
using Clara.Analysis;
using Clara.Storage;

namespace Clara.Mapping
{
    public class RangeField<TValue> : Field
        where TValue : struct, IComparable<TValue>
    {
        public RangeField(TValue minValue, TValue maxValue, bool isFilterable = false, bool isFacetable = false, bool isSortable = false)
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

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderStore tokenEncoderStore, ISynonymMap? synonymMap)
        {
            return new RangeFieldStoreBuilder<TValue>(this);
        }
    }
}
