using System.Text;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AllValuesExpression : ValuesExpression
    {
        internal AllValuesExpression(HashSetSlim<string> values)
            : base(values)
        {
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("ALL(");

            var isFirst = true;

            foreach (var value in this.Values)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(value);
                builder.Append('"');

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
