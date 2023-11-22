using Clara.Mapping;

namespace Clara.Querying
{
    public class RangeFilterExpression<TValue> : FilterExpression
        where TValue : struct, IComparable<TValue>
    {
        public RangeFilterExpression(RangeField<TValue> field, TValue? valueFrom, TValue? valueTo)
            : base(field)
        {
            if (valueFrom is not null)
            {
                if (!(field.MinValue.CompareTo(valueFrom.Value) <= 0 && valueFrom.Value.CompareTo(field.MaxValue) <= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(valueFrom));
                }
            }

            if (valueTo is not null)
            {
                if (!(field.MinValue.CompareTo(valueTo.Value) <= 0 && valueTo.Value.CompareTo(field.MaxValue) <= 0))
                {
                    throw new ArgumentOutOfRangeException(nameof(valueFrom));
                }
            }

            if (valueFrom is not null && valueTo is not null)
            {
                if (valueFrom.Value.CompareTo(valueTo.Value) > 0)
                {
                    throw new ArgumentException("From value has to be less or equal to to value.", nameof(valueFrom));
                }
            }

            this.ValueFrom = valueFrom;
            this.ValueTo = valueTo;
        }

        public TValue? ValueFrom { get; }

        public TValue? ValueTo { get; }

        internal override bool IsEmpty
        {
            get
            {
                return this.ValueFrom is null && this.ValueTo is null;
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
