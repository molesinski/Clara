using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFilterExpression : FilterExpression
    {
        public HierarchyFilterExpression(HierarchyField field, FilterMode filterMode, string? value)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.Values = FilterValuesHelper.GetValues(value);
        }

        public HierarchyFilterExpression(HierarchyField field, FilterMode filterMode, IEnumerable<string?>? values)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.Values = FilterValuesHelper.GetValues(values);
        }

        public FilterMode FilterMode { get; }

        public IReadOnlyCollection<string> Values { get; }

        internal override bool IsEmpty
        {
            get
            {
                return this.Values.Count == 0;
            }
        }

        internal override bool IsBranchingRequiredForFaceting
        {
            get
            {
                return false;
            }
        }
    }
}
