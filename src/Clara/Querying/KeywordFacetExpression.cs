using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetExpression : TokenFacetExpression<KeywordFacetValue>
    {
        public KeywordFacetExpression(KeywordField field)
            : base(field)
        {
        }
    }
}
