using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFilterExpression : TokenFilterExpression
    {
        public KeywordFilterExpression(KeywordField field, ValuesExpression valuesExpression)
            : base(field, valuesExpression)
        {
        }
    }
}
