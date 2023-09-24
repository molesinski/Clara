using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetExpression : FacetExpression
    {
        public KeywordFacetExpression(KeywordField field)
            : base(field)
        {
        }
    }
}
