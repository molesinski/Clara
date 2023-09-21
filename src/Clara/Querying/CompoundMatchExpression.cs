using Clara.Utils;

namespace Clara.Querying
{
    public abstract class CompoundMatchExpression : MatchExpression
    {
        private readonly ListSlim<MatchExpression> expressions;

        internal CompoundMatchExpression(ListSlim<MatchExpression> expressions)
        {
            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.expressions = expressions;
        }

        public IReadOnlyCollection<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }
    }
}
