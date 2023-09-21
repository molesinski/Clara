using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AnyValuesExpression : ValuesExpression
    {
        internal AnyValuesExpression(HashSetSlim<string> values)
            : base(values)
        {
        }
    }
}
