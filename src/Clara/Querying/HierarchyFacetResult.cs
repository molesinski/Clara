using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetResult : TokenFacetResult<HierarchyValue>
    {
        public HierarchyFacetResult(HierarchyField field, IEnumerable<HierarchyValue> values)
            : base(field, values)
        {
        }
    }
}
