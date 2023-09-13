namespace Clara.Querying
{
    public sealed class EmptyMatchExpression : MatchExpression
    {
        private EmptyMatchExpression()
        {
        }

        internal static EmptyMatchExpression Instance { get; } = new EmptyMatchExpression();
    }
}
