using Clara.Analysis.MatchExpressions;

namespace Clara.Analysis
{
    public readonly record struct SearchTerm
    {
        public SearchTerm(string token, Offset offset)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.Token = token;
            this.Offset = offset;
        }

        public SearchTerm(MatchExpression expression, Offset offset)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            this.Expression = expression;
            this.Offset = offset;
        }

        public string? Token { get; }

        public MatchExpression? Expression { get; }

        public Offset Offset { get; }
    }
}
