namespace Clara.Querying
{
    public sealed class EmptyMatchExpression : MatchExpression
    {
        private EmptyMatchExpression()
        {
        }

        internal static MatchExpression Instance { get; } = new EmptyMatchExpression();
    }
}
