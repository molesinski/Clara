using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFacetResult<TValue> : FacetResult
        where TValue : notnull
    {
        protected internal TokenFacetResult(TokenField field, IEnumerable<TValue> values)
            : base(field)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.Values = values;
        }

        public IEnumerable<TValue> Values { get; }
    }
}
