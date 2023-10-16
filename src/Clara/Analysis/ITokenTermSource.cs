namespace Clara.Analysis
{
    public interface ITokenTermSource
    {
        IEnumerable<TokenTerm> GetTerms(string text);
    }
}
