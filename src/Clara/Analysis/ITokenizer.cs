namespace Clara.Analysis
{
    public interface ITokenizer
    {
        IDisposableEnumerable<Token> GetTokens(string text);
    }
}
