using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AllTokensMatchExpression : TokensMatchExpression
    {
        internal AllTokensMatchExpression(ListSlim<string> tokens)
            : base(tokens)
        {
        }
    }
}
