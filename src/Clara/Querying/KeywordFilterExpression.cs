using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFilterExpression : TokenFilterExpression
    {
        public KeywordFilterExpression(KeywordField field, MatchExpression matchExpression)
            : base(field, matchExpression)
        {
        }
    }
}
