using System;
using Clara.Mapping;

namespace Clara.Querying
{
    public class RangeFacetExpression<TValue> : FacetExpression
        where TValue : struct, IComparable<TValue>
    {
        public RangeFacetExpression(RangeField<TValue> field)
            : base(field)
        {
        }

        public virtual RangeFacetResult<TValue> CreateResult(TValue min, TValue max)
        {
            return new RangeFacetResult<TValue>((RangeField<TValue>)this.Field, min, max);
        }
    }
}
