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
            var length = span.Length;

            for (var i = 0; i < length; i++)
            {
                span[i] = char.ToLowerInvariant(span[i]);
            }

            return next(token);
        }
    }
}
