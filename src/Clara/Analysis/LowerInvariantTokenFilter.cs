namespace Clara.Analysis
{
    public sealed class LowerInvariantTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            var length = token.Length;

            for (var i = 0; i < length; i++)
            {
                token[i] = char.ToLowerInvariant(token[i]);
            }

            return next(token);
        }
    }
}
