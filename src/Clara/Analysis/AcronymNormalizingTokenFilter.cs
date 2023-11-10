namespace Clara.Analysis
{
    public sealed class AcronymNormalizingTokenFilter : ITokenFilter
    {
        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (token.Length > 2 && token.Length < Token.MaximumLength && token.Length % 2 == 1)
            {
                var span = token.AsReadOnlySpan();
                var length = span.Length;

                var isAcronym = true;

                for (var i = 0; i < length; i += 2)
                {
                    if (!(char.IsLetter(span[i]) && (i + 1 == length || span[i + 1] == '.')))
                    {
                        isAcronym = false;
                        break;
                    }
                }

                if (isAcronym)
                {
                    token.Append(".");
                }
            }

            return next(token);
        }
    }
}
