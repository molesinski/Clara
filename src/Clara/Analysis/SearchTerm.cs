using Clara.Analysis.MatchExpressions;

namespace Clara.Analysis
{
    public readonly record struct SearchTerm
    {
        public SearchTerm(int position, string token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.Position = position;
            this.Token = token;
        }

        public SearchTerm(int position, MatchExpression expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            this.Position = position;
            this.Expression = expression;
        }

        public int Position { get; }

        public string? Token { get; }

        public MatchExpression? Expression { get; }
    }
}
