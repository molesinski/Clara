namespace Clara.Analysis
{
    public class EnglishPossesiveTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var length = token.Length;

            if (length >= 2)
            {
                if (token[length - 1] == 's' || token[length - 1] == 'S')
                {
                    if (token[length - 2] == '\'' || token[length - 2] == '\u2019' || token[length - 2] == '\uFF07')
                    {
                        token.Remove(length - 2);
                    }
                }
            }

            return next(token);
        }
    }
}
