using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class RangeFacetResult<TValue> : FacetResult
        where TValue : struct, IComparable<TValue>
    {
        internal RangeFacetResult(RangeField<TValue> field, TValue minValue, TValue maxValue)
            : base(field)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public TValue MinValue { get; }

        public TValue MaxValue { get; }
    }
}
