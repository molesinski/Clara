using Clara.Mapping;
using Clara.Querying;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap : IAnalyzer, IMatchExpressionFilter
    {
        TextField Field { get; }
    }
}
