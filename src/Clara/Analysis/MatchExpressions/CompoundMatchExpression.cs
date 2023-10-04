using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class CompoundMatchExpression : MatchExpression
    {
        internal CompoundMatchExpression(ScoringMode scoringMode, ListSlim<MatchExpression> expressions)
        {
            if (scoringMode != ScoringMode.Sum && scoringMode != ScoringMode.Max)
            {
                throw new ArgumentException("Illegal scoring mode enum value.", nameof(scoringMode));
            }

            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.ScoringMode = scoringMode;
            this.Expressions = expressions;
        }

        public ScoringMode ScoringMode { get; }

        public IReadOnlyCollection<MatchExpression> Expressions { get; }
    }
}
