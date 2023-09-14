using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetExpression : TokenFacetExpression<HierarchyFacetValue>
    {
        public HierarchyFacetExpression(HierarchyField field)
            : base(field)
        {
        }
    }
}
