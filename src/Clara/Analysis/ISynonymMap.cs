namespace Clara.Analysis
{
    public interface ISynonymMap
    {
        IAnalyzer Analyzer { get; }

        ITokenTermSource CreateIndexTokenTermSource();

        ITokenTermSource CreateSearchTokenTermSource();
    }
}
