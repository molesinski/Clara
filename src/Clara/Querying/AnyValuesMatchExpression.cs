namespace Clara.Querying
{
    public sealed class AnyValuesMatchExpression : ValuesMatchExpression
    {
        internal AnyValuesMatchExpression(List<string> values)
            : base(values)
        {
        }
    }
}
