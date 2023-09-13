namespace Clara.Querying
{
    public sealed class AllValuesExpression : ValuesExpression
    {
        internal AllValuesExpression(IReadOnlyCollection<string> values)
            : base(values)
        {
        }
    }
}
