using Clara.Mapping;
using Clara.Querying;

namespace Clara.Analysis
{
    public interface ISynonymMap : ITokenFilter, IMatchExpressionFilter
    {
        TextField Field { get; }
    }
}
