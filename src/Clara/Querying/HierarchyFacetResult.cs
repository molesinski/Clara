using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetResult : TokenFacetResult<HierarchyFacetValue>
    {
        public HierarchyFacetResult(HierarchyField field, IEnumerable<HierarchyFacetValue> values)
            : base(field, values)
        {
        }
    }
}
