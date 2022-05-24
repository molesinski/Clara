using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class AllValuesMatchExpression : ValuesMatchExpression
    {
        internal AllValuesMatchExpression(HashSet<string> values)
            : base(values)
        {
        }
    }
}
