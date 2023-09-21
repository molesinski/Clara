using Clara.Utils;

namespace Clara.Querying
{
    public sealed class EmptyValuesMatchExpression : ValuesMatchExpression
    {
        private EmptyValuesMatchExpression()
            : base(new HashSetSlim<string>())
        {
        }

        internal static EmptyValuesMatchExpression Instance { get; } = new EmptyValuesMatchExpression();
    }
}
