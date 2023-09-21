using Clara.Mapping;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap : IAnalyzer, IMatchExpressionFilter
    {
        TextField Field { get; }
    }
}
