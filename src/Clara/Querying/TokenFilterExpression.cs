using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFilterExpression : FilterExpression
    {
        internal TokenFilterExpression(TokenField field, ValuesExpression valuesExpression)
            : base(field)
        {
            if (valuesExpression is null)
            {
                throw new ArgumentNullException(nameof(valuesExpression));
            }

            this.ValuesExpression = valuesExpression;
        }

        public ValuesExpression ValuesExpression { get; }

        internal override bool IsEmpty
        {
            get
            {
                return this.ValuesExpression is EmptyValuesExpression;
            }
        }

        internal override bool IsBranchingRequiredForFaceting
        {
            get
            {
                return this.ValuesExpression is AnyValuesExpression;
            }
        }
    }
}
