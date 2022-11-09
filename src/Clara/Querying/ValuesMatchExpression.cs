namespace Clara.Querying
{
    public abstract class ValuesMatchExpression : MatchExpression
    {
        private readonly IReadOnlyList<string> values;

        protected internal ValuesMatchExpression(IReadOnlyList<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.values = values;
        }

        public IReadOnlyList<string> Values
        {
            get
            {
                return this.values;
            }
        }
    }
}
