using System.Text;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class EmptyMatchExpression : MatchExpression
    {
        private EmptyMatchExpression()
        {
        }

        public override ScoreAggregation ScoreAggregation
        {
            get
            {
                throw new InvalidOperationException("Unable to retrieve scoring aggregation value for empty match expression.");
            }
        }

        internal static MatchExpression Instance { get; } = new EmptyMatchExpression();

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("EMPTY");
        }
    }
}
