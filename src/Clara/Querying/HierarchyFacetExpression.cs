using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetExpression : FacetExpression
    {
        public HierarchyFacetExpression(HierarchyField field)
            : base(field)
        {
        }
    }
}
