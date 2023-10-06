using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AnyTokensMatchExpression : TokensMatchExpression
    {
        internal AnyTokensMatchExpression(ScoringMode scoringMode, ListSlim<string> tokens)
            : base(scoringMode, tokens)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var token in (ListSlim<string>)this.Tokens)
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

            foreach (var token in (ListSlim<string>)this.Tokens)
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
