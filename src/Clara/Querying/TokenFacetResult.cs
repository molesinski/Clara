using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFacetResult<TValue> : FacetResult
        where TValue : notnull
    {
        internal TokenFacetResult(TokenField field)
            : base(field)
        {
        }

        public abstract IEnumerable<TValue> Values { get; }
    }
}
