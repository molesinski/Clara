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
    }
}
