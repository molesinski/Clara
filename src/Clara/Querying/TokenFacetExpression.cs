using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFacetExpression<TValue> : FacetExpression
        where TValue : notnull
    {
        protected internal TokenFacetExpression(TokenField field)
            : base(field)
        {
        }

        public abstract FacetResult CreateResult(IEnumerable<TValue> values);
    }
}
