using Clara.Querying;

namespace Clara.Analysis
{
    public interface IMatchExpressionFilter
    {
        MatchExpression Process(MatchExpression matchExpression);
    }
}
