namespace Clara.Analysis
{
    public sealed class EnglishPossessiveTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (token.Length >= 2)
            {
                var span = token.AsReadOnlySpan();
                var length = span.Length;

                if (span[length - 1] == 's' || span[length - 1] == 'S')
                {
                    if (span[length - 2] == '\'' || span[length - 2] == '\u2019' || span[length - 2] == '\uFF07')
                    {
                        token.Remove(length - 2);
                    }
                }
            }

            return next(token);
        }
    }
}
