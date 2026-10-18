namespace Clara.Analysis.Synonyms
{
    public interface ISynonymMap
    {
        IAnalyzer Analyzer { get; }

        ITokenTermSource CreateTokenTermSource();

        IPhraseTermSource CreatePhraseTermSource();
    }
}
