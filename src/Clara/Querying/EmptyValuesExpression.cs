namespace Clara.Querying
{
    public sealed class EmptyValuesExpression : ValuesExpression
    {
        private EmptyValuesExpression()
            : base(Array.Empty<string>())
        {
        }

        internal static EmptyValuesExpression Instance { get; } = new EmptyValuesExpression();
    }
}
