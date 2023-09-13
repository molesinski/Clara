namespace Clara.Querying
{
    public sealed class AnyValuesExpression : ValuesExpression
    {
        internal AnyValuesExpression(IReadOnlyCollection<string> values)
            : base(values)
        {
        }
    }
}
