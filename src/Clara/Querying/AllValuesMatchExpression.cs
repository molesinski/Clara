using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AllValuesMatchExpression : ValuesMatchExpression
    {
        internal AllValuesMatchExpression(HashSetSlim<string> values)
            : base(values)
        {
        }
    }
}
