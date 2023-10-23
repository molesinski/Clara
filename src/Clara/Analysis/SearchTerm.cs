using Clara.Analysis.MatchExpressions;

namespace Clara.Analysis
{
    public readonly record struct SearchTerm
    {
        public SearchTerm(string token, TokenPosition position)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.Token = token;
            this.Position = position;
        }

        public SearchTerm(MatchExpression expression, TokenPosition position)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            this.Expression = expression;
            this.Position = position;
        }

        public string? Token { get; }

        public MatchExpression? Expression { get; }

        public TokenPosition Position { get; }
    }
}
