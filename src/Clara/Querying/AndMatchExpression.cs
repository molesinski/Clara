using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AndMatchExpression : CompoundMatchExpression
    {
        internal AndMatchExpression(ListSlim<MatchExpression> expressions)
            : base(expressions)
        {
        }
    }
}
