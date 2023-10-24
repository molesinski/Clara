using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class OrMatchExpression : ComplexMatchExpression
    {
        internal OrMatchExpression(ListSlim<MatchExpression> expressions)
            : base(expressions)
        {
        }

        public override bool Matches(IReadOnlyCollection<string> tokens)
        {
            for (var i = 0; i < this.Expressions.Count; i++)
            {
                if (this.Expressions[i].Matches(tokens))
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

            for (var i = 0; i < this.Expressions.Count; i++)
            {
                if (!isFirst)
                {
                    builder.Append(" OR ");
                }

                this.Expressions[i].ToString(builder);

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
