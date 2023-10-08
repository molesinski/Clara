namespace Clara.Analysis
{
    public interface IAnalyzer
    {
        IEnumerable<Token> GetTokens(string text);
    }
}
