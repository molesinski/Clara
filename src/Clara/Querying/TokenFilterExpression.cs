using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFilterExpression : FilterExpression
    {
        protected internal TokenFilterExpression(TokenField field, MatchExpression matchExpression)
            : base(field)
        {
            if (matchExpression is null)
            {
                throw new ArgumentNullException(nameof(matchExpression));
            }

            this.MatchExpression = matchExpression;
        }

        public MatchExpression MatchExpression { get; }

        public override bool IsEmpty
        {
            get
            {
                return this.MatchExpression is EmptyMatchExpression;
            }
        }

        internal override bool IsBranchingRequiredForFaceting
        {
            get
            {
                return this.MatchExpression is AnyValuesMatchExpression;
            }
        }
    }
}
