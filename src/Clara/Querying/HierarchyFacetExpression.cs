using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetExpression : TokenFacetExpression<HierarchyFacetValue>
    {
        public HierarchyFacetExpression(HierarchyField field)
            : base(field)
        {
        }

        public override FacetResult CreateResult(IEnumerable<HierarchyFacetValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return new HierarchyFacetResult((HierarchyField)this.Field, values);
        }
    }
}
