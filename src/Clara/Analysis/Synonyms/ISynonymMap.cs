using Clara.Querying;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap : IAnalyzer
    {
        IAnalyzer Analyzer { get; }

        MatchExpression Process(MatchExpression matchExpression);
    }
}
