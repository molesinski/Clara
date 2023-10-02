using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class RangeFacetResult<TValue> : FacetResult
        where TValue : struct, IComparable<TValue>
    {
        internal RangeFacetResult(RangeField<TValue> field, TValue min, TValue max)
            : base(field)
        {
            this.Min = min;
            this.Max = max;
        }

        public TValue Min { get; }

        public TValue Max { get; }
    }
}
