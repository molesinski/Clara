namespace Clara.Querying
{
    public abstract class ValuesExpression
    {
        private readonly IReadOnlyCollection<string> values;

        protected internal ValuesExpression(IReadOnlyCollection<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.values = values;
        }

        public IReadOnlyCollection<string> Values
        {
            get
            {
                return this.values;
            }
        }
    }
}
