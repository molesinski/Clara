using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AndMatchExpression : ComplexMatchExpression
    {
        internal AndMatchExpression(ScoringMode scoringMode, ListSlim<MatchExpression> expressions)
            : base(scoringMode, expressions)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var expression in (ListSlim<MatchExpression>)this.Expressions)
            {
                if (!expression.IsMatching(tokens))
                {
                    return false;
                }
            }

            return true;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('(');

            var isFirst = true;

            foreach (var expression in (ListSlim<MatchExpression>)this.Expressions)
            {
                if (!isFirst)
                {
                    builder.Append(" AND ");
                }

                expression.ToString(builder);

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
