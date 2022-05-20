using System;
using Clara.Mapping;

namespace Clara.Querying
{
    public class RangeFacetResult<TValue> : FacetResult
        where TValue : struct, IComparable<TValue>
    {
        public RangeFacetResult(RangeField<TValue> field, TValue min, TValue max)
            : base(field)
        {
            this.Min = min;
            this.Max = max;
        }

        public TValue Min { get; }

        public TValue Max { get; }
    }
}
