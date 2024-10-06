namespace Clara.Analysis
{
    public interface IPhraseTermSource
    {
        IEnumerable<PhraseTerm> GetTerms(string text);
    }
}
