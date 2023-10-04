using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class OrMatchExpression : CompoundMatchExpression
    {
        internal OrMatchExpression(ScoringMode scoringMode, ListSlim<MatchExpression> expressions)
            : base(scoringMode, expressions)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var expression in this.Expressions)
            {
                if (expression.IsMatching(tokens))
                {
                    return true;
                }
            }

            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('(');

            var isFirst = true;

            foreach (var expression in this.Expressions)
            {
                if (!isFirst)
                {
                    builder.Append(" OR ");
                }

                expression.ToString(builder);

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
