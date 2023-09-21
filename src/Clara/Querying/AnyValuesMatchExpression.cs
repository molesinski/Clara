using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AnyValuesMatchExpression : ValuesMatchExpression
    {
        internal AnyValuesMatchExpression(HashSetSlim<string> values)
            : base(values)
        {
        }
    }
}
