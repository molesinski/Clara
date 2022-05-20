using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetExpression : TokenFacetExpression<HierarchyValue>
    {
        public HierarchyFacetExpression(HierarchyField field, IComparer<HierarchyValue>? comparer = null)
            : base(field, comparer ?? HierarchyValueComparer.Default)
        {
        }

        public override FacetResult CreateResult(IEnumerable<HierarchyValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return new HierarchyFacetResult((HierarchyField)this.Field, values);
        }
    }
}
