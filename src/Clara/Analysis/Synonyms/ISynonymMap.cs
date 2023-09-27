namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap : IAnalyzer, IMatchExpressionFilter
    {
        IAnalyzer Analyzer { get; }
    }
}
