using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class ComplexMatchExpression : MatchExpression
    {
        private readonly ListSlim<MatchExpression> expressions;

        internal ComplexMatchExpression(ScoreAggregation scoreAggregation, ListSlim<MatchExpression> expressions)
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

        public ScoreAggregation ScoreAggregation { get; }

        public IReadOnlyList<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }
    }
}
