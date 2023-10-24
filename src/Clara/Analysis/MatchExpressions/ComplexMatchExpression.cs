using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class ComplexMatchExpression : MatchExpression
    {
        private readonly ListSlim<MatchExpression> expressions;

        internal ComplexMatchExpression(ListSlim<MatchExpression> expressions)
        {
            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.expressions = expressions;
        }

        public IReadOnlyList<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }
    }
}
