using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class OrMatchExpression : ComplexMatchExpression
    {
        internal OrMatchExpression(ScoreAggregation scoreAggregation, ListSlim<MatchExpression> expressions, bool isLazy)
            : base(scoreAggregation, expressions)
        {
            this.IsLazy = isLazy;
        }

        public bool IsLazy { get; }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            for (var i = 0; i < this.Expressions.Count; i++)
            {
                if (this.Expressions[i].IsMatching(tokens))
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
