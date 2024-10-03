namespace Clara.Analysis
{
    public interface ISynonymTermSource
    {
        IEnumerable<SynonymTerm> GetTerms(string text);
    }
}
