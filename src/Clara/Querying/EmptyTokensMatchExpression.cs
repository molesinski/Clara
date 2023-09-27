using System.Text;
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

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("EMPTY");
        }
    }
}
