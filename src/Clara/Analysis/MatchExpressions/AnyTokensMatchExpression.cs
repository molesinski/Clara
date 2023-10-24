using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AnyTokensMatchExpression : TokensMatchExpression
    {
        internal AnyTokensMatchExpression(ListSlim<string> tokens)
            : base(tokens)
        {
        }

        public override bool Matches(IReadOnlyCollection<string> tokens)
        {
            for (var i = 0; i < this.Tokens.Count; i++)
            {
                if (tokens.Contains(this.Tokens[i]))
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

            for (var i = 0; i < this.Tokens.Count; i++)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(this.Tokens[i]);
                builder.Append('"');

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
