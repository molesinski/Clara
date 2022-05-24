using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class AnyValuesMatchExpression : ValuesMatchExpression
    {
        internal AnyValuesMatchExpression(HashSet<string> values)
            : base(values)
        {
        }
    }
}
