namespace Clara.Querying
{
    public sealed class AndMatchExpression : MatchExpression
    {
        private readonly List<MatchExpression> expressions;

        internal AndMatchExpression(List<MatchExpression> expressions)
        {
            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.expressions = expressions;
        }

        public IEnumerable<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }
    }
}
