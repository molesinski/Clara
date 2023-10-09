namespace Clara.Analysis
{
    public sealed class DigitsStopTokenFilter : ITokenFilter
    {
        public void Process(in Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var span = token.AsReadOnlySpan();
            var length = span.Length;

            for (var i = 0; i < length; i++)
            {
                if (char.IsDigit(span[i]))
                {
                    token.Clear();
                    return;
                }
            }

            next(in token);
        }
    }
}
