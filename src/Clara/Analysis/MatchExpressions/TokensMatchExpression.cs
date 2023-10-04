using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class TokensMatchExpression : MatchExpression
    {
        internal TokensMatchExpression(ScoringMode scoringMode, ListSlim<string> tokens)
        {
            if (scoringMode != ScoringMode.Sum && scoringMode != ScoringMode.Max)
            {
                throw new ArgumentException("Illegal scoring mode enum value.", nameof(scoringMode));
            }

            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.ScoringMode = scoringMode;
            this.Tokens = tokens;
        }

        public ScoringMode ScoringMode { get; }

        public IReadOnlyCollection<string> Tokens { get; }
    }
}
