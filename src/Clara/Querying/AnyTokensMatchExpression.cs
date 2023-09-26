using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AnyTokensMatchExpression : TokensMatchExpression
    {
        internal AnyTokensMatchExpression(ListSlim<string> tokens)
            : base(tokens)
        {
        }
    }
}
