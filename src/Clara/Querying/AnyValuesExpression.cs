using System.Text;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class AnyValuesExpression : ValuesExpression
    {
        internal AnyValuesExpression(HashSetSlim<string> values)
            : base(values)
        {
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("ANY(");

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
