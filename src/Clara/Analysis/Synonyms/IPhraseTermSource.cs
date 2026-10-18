namespace Clara.Analysis.Synonyms
{
    public interface IPhraseTermSource
    {
        IEnumerable<PhraseTerm> GetTerms(string text);
    }
}
