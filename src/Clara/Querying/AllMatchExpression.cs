namespace Clara.Querying
{
    public sealed class AllMatchExpression : MatchExpression
    {
        private readonly IReadOnlyCollection<string> values;

        internal AllMatchExpression(IReadOnlyCollection<string> values)
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
