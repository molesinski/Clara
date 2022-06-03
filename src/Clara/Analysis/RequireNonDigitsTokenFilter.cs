namespace Clara.Analysis
{
    public class RequireNonDigitsTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            var length = token.Length;

            for (var i = 0; i < length; i++)
            {
                if (char.IsDigit(token[i]))
                {
                    return token;
                }
            }

            return next(token);
        }
    }
}
