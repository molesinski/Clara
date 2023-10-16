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

            var span = token.AsSpan();

            for (var i = 0; i < span.Length; i++)
            {
                span[i] = char.ToLowerInvariant(span[i]);
            }

            return next(token);
        }
    }
}
