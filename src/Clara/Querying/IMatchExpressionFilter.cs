namespace Clara.Querying
{
    public interface IMatchExpressionFilter
    {
        MatchExpression Process(MatchExpression matchExpression);
    }
}
