using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AndMatchExpression : MatchExpression
    {
        private readonly ListSlim<MatchExpression> expressions;

        internal AndMatchExpression(ScoreAggregation scoreAggregation, ListSlim<MatchExpression> expressions)
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

            foreach (var expression in this.expressions)
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
