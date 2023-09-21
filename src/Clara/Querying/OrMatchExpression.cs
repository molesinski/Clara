using Clara.Utils;

namespace Clara.Querying
{
    public sealed class OrMatchExpression : CompoundMatchExpression
    {
        internal OrMatchExpression(ListSlim<MatchExpression> expressions)
            : base(expressions)
        {
        }
    }
}
