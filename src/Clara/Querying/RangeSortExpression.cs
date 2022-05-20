using System;
using Clara.Mapping;

namespace Clara.Querying
{
    public class RangeSortExpression<TValue> : SortExpression
        where TValue : struct, IComparable<TValue>
    {
        public RangeSortExpression(RangeField<TValue> field, SortDirection direction)
            : base(field, direction)
        {
        }
    }
}
