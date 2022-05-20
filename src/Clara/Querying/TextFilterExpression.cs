using System;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class TextFilterExpression : TokenFilterExpression
    {
        public TextFilterExpression(TextField field, MatchExpression matchExpression)
            : base(field, matchExpression)
        {
        }
    }
}
