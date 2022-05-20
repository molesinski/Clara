namespace Clara.Querying
{
    public interface IMatchExpressionFilter
    {
        MatchExpression Filter(MatchExpression matchExpression);
    }
}
