using System.Text;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AndMatchExpression : CompoundMatchExpression
    {
        internal AndMatchExpression(ListSlim<MatchExpression> expressions)
            : base(expressions)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var expression in this.Expressions)
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

            foreach (var expression in this.Expressions)
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
