namespace Clara.Analysis
{
    public interface ITokenizer
    {
        IEnumerable<Token> GetTokens(string text);
    }
}
