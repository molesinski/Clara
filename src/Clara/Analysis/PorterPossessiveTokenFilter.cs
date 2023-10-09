namespace Clara.Analysis
{
    public sealed class PorterPossessiveTokenFilter : ITokenFilter
    {
        public void Process(in Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var span = token.AsReadOnlySpan();
            var length = span.Length;

            if (length >= 2)
            {
                if (span[length - 1] == 's' || span[length - 1] == 'S')
                {
                    if (span[length - 2] == '\'' || span[length - 2] == '\u2019' || span[length - 2] == '\uFF07')
                    {
                        token.Remove(length - 2);
                    }
                }
            }

            next(in token);
        }
    }
}
