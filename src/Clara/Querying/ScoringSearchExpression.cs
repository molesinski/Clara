using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class ScoringSearchExpression : SearchExpression
    {
        internal ScoringSearchExpression(Field field)
            : base(field)
        {
        }
    }
}
