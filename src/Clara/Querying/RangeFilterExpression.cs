using Clara.Mapping;

namespace Clara.Querying
{
    public class RangeFilterExpression<TValue> : FilterExpression
        where TValue : struct, IComparable<TValue>
    {
        public RangeFilterExpression(RangeField<TValue> field, TValue? from, TValue? to)
            : base(field)
        {
            this.From = from;
            this.To = to;
        }

        public TValue? From { get; }

        public TValue? To { get; }

        internal override bool IsEmpty
        {
            get
            {
                return this.From is null && this.To is null;
            }
        }

        internal override bool IsBranchingRequiredForFaceting
        {
            get
            {
                return true;
            }
        }
    }
}
