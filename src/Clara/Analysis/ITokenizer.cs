namespace Clara.Analysis
{
    public interface ITokenizer : IEquatable<ITokenizer>
    {
        IEnumerable<Token> GetTokens(string text);
    }
}
