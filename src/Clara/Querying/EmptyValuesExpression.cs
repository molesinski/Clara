using System.Text;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class EmptyValuesExpression : ValuesExpression
    {
        private EmptyValuesExpression()
            : base(new HashSetSlim<string>())
        {
        }

        internal static EmptyValuesExpression Instance { get; } = new EmptyValuesExpression();

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("EMPTY");
        }
    }
}
