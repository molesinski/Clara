using Clara.Utils;

namespace Clara.Querying
{
    public sealed class EmptyTokensMatchExpression : TokensMatchExpression
    {
        private EmptyTokensMatchExpression()
            : base(new ListSlim<string>())
        {
        }

        internal static EmptyTokensMatchExpression Instance { get; } = new EmptyTokensMatchExpression();
    }
}
