using Clara.Mapping;
using Clara.Querying;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap : ITokenFilter, IMatchExpressionFilter
    {
        TextField Field { get; }
    }
}
