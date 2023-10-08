using Clara.Analysis.MatchExpressions;

namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap : IAnalyzer
    {
        IAnalyzer Analyzer { get; }

        MatchExpression Process(MatchExpression matchExpression);

        Token? ToReadOnly(Token token);
    }
}
