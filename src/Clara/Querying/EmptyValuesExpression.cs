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
    }
}
