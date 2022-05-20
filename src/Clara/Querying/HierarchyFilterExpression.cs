using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFilterExpression : TokenFilterExpression
    {
        public HierarchyFilterExpression(HierarchyField field, MatchExpression matchExpression)
            : base(field, matchExpression)
        {
        }
    }
}
