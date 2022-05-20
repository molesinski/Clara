using System;

namespace Clara.Mapping
{
    internal sealed class RangeFieldValue<TValue> : FieldValue
        where TValue : struct, IComparable<TValue>
    {
        public RangeFieldValue(RangeField<TValue> field, TValue[] values)
            : base(field)
        {
            this.Values = values;
        }

        public TValue[] Values { get; }
    }
}
