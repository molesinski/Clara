namespace Clara.Analysis
{
    public interface ITokenFilter
    {
        Token Process(Token token, TokenFilterDelegate next);
    }
}
