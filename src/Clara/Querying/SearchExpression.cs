using Clara.Storage;

namespace Clara.Querying
{
    public abstract class SearchExpression
    {
        internal SearchExpression()
        {
        }

        internal abstract bool IsEmpty { get; }

        internal abstract DocumentScoring Search(Index index);
    }
}
