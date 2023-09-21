using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AllValuesExpression : ValuesExpression
    {
        internal AllValuesExpression(HashSetSlim<string> values)
            : base(values)
        {
        }
    }
}
