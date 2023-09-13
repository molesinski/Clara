using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFilterExpression : TokenFilterExpression
    {
        public HierarchyFilterExpression(HierarchyField field, ValuesExpression valuesExpression)
            : base(field, valuesExpression)
        {
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
