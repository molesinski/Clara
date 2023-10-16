namespace Clara.Analysis
{
    public class AcronymFoldingTokenFilter : ITokenFilter
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
                    for (var i = 0; i < length / 2; i++)
                    {
                        token.Delete(i + 1, 1);
                    }
                }
            }

            return next(token);
        }
    }
}
