using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFacetExpression<TValue> : FacetExpression
        where TValue : notnull
    {
        internal TokenFacetExpression(TokenField field)
            : base(field)
        {
        }
    }
}
