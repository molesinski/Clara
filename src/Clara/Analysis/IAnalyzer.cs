namespace Clara.Analysis
{
    public interface IAnalyzer
    {
        IDisposableEnumerable<string> GetTokens(string text);
    }
}
