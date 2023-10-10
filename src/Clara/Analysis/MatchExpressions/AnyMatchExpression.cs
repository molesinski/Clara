using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AnyMatchExpression : MatchExpression
    {
        private readonly ListSlim<string> tokens;

        internal AnyMatchExpression(ScoreAggregation scoreAggregation, ListSlim<string> tokens)
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

        public override ScoreAggregation ScoreAggregation { get; }

        public IReadOnlyList<string> Tokens
        {
            get
            {
                return this.tokens;
            }
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var token in this.tokens)
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

            foreach (var token in this.tokens)
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
