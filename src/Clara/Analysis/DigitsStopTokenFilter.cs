namespace Clara.Analysis
{
    public class DigitsStopTokenFilter : ITokenFilter
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
                if (char.IsDigit(token[i]))
                {
                    return default;
                }
            }

            return next(token);
        }
    }
}
