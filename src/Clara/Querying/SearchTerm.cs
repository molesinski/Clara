using Clara.Analysis.MatchExpressions;

namespace Clara.Querying
{
    public readonly record struct SearchTerm
    {
        public SearchTerm(int ordinal, string token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.Ordinal = ordinal;
            this.Token = token;
        }

        public SearchTerm(int ordinal, MatchExpression expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            this.Ordinal = ordinal;
            this.Expression = expression;
        }

        public int Ordinal { get; }

        public string? Token { get; }

        public MatchExpression? Expression { get; }
    }
}
