namespace Clara.Analysis
{
    public sealed class LowerInvariantTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var length = token.Length;

            for (var i = 0; i < length; i++)
            {
                token[i] = char.ToLowerInvariant(token[i]);
            }

            return next(token);
        }
    }
}
