namespace Clara.Analysis
{
    public interface ITokenFilter
    {
#pragma warning disable CA1716 // Identifiers should not match keywords
        Token Process(Token token, TokenFilterDelegate next);
#pragma warning restore CA1716 // Identifiers should not match keywords
    }
}
