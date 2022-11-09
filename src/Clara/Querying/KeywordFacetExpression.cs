using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetExpression : TokenFacetExpression<KeywordFacetValue>
    {
        public KeywordFacetExpression(KeywordField field)
            : base(field)
        {
        }

        public override FacetResult CreateResult(IEnumerable<KeywordFacetValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return new KeywordFacetResult((KeywordField)this.Field, values);
        }
    }
}
