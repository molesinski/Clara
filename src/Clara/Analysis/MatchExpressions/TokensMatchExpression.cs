using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class TokensMatchExpression : MatchExpression
    {
        private readonly ListSlim<string> tokens;

        internal TokensMatchExpression(ScoreAggregation scoreAggregation, ListSlim<string> tokens)
        {
            if (scoreAggregation != ScoreAggregation.Sum && scoreAggregation != ScoreAggregation.Max)
            {
                throw new ArgumentException("Illegal score aggregation enum value.", nameof(scoreAggregation));
            }

            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.ScoreAggregation = scoreAggregation;
            this.tokens = tokens;
        }

        public ScoreAggregation ScoreAggregation { get; }

        public IReadOnlyList<string> Tokens
        {
            get
            {
                return this.tokens;
            }
        }
    }
}
