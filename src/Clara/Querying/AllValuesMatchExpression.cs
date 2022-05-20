using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class AllValuesMatchExpression : ValuesMatchExpression
    {
        internal AllValuesMatchExpression(List<string> values)
            : base(values)
        {
        }
    }
}
