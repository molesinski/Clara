namespace Clara.Analysis
{
    public interface ISynonymMap
    {
        IAnalyzer Analyzer { get; }

        ITokenTermSource CreateTokenTermSource();

        IPhraseTermSource CreatePhraseTermSource();
    }
}
