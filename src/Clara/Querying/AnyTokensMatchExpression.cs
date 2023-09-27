using System.Text;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AnyTokensMatchExpression : TokensMatchExpression
    {
        internal AnyTokensMatchExpression(ListSlim<string> tokens)
            : base(tokens)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var token in this.Tokens)
            {
                if (tokens.Contains(token))
                {
                    return true;
                }
            }

            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("ANY(");

            var isFirst = true;

            foreach (var token in this.Tokens)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(token);
                builder.Append('"');

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
