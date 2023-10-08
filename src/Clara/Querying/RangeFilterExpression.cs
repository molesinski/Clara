using Clara.Mapping;

namespace Clara.Querying
{
    public class RangeFilterExpression<TValue> : FilterExpression
        where TValue : struct, IComparable<TValue>
    {
        public RangeFilterExpression(RangeField<TValue> field, TValue? from, TValue? to)
            : base(field)
        {
            if (from is not null)
            {
                if (!(field.MinValue.CompareTo(from.Value) <= 0 && from.Value.CompareTo(field.MaxValue) <= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(from));
                }
            }

            if (to is not null)
            {
                if (!(field.MinValue.CompareTo(to.Value) <= 0 && to.Value.CompareTo(field.MaxValue) <= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(from));
                }
            }

            if (from is not null && to is not null)
            {
                if (!(from.Value.CompareTo(to.Value) <= 0))
                {
                    throw new ArgumentException("From value has to be less or equal to to value.", nameof(from));
                }
            }

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
