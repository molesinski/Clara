namespace Clara.Analysis
{
    public sealed class DigitsKeywordTokenFilter : ITokenFilter
    {
        public void Process(ref Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var span = token.AsReadOnlySpan();

            for (var i = 0; i < span.Length; i++)
            {
                if (char.IsDigit(span[i]))
                {
                    return;
                }
            }

            next(ref token);
        }
    }
}
