using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class OrMatchExpression : MatchExpression
    {
        private readonly ListSlim<MatchExpression> expressions;

        internal OrMatchExpression(ScoreAggregation scoreAggregation, ListSlim<MatchExpression> expressions)
        {
            if (scoreAggregation != ScoreAggregation.Sum && scoreAggregation != ScoreAggregation.Max)
            {
                throw new ArgumentException("Illegal score aggregation enum value.", nameof(scoreAggregation));
            }

            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.ScoreAggregation = scoreAggregation;
            this.expressions = expressions;
        }

        public override ScoreAggregation ScoreAggregation { get; }

        public IReadOnlyList<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var expression in this.expressions)
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

            foreach (var expression in this.expressions)
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
