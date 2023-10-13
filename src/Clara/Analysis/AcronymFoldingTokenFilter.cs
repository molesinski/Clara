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

            if (token.Length >= 3 && token.Length % 2 == 1)
            {
                var span = token.AsReadOnlySpan();
                var length = span.Length;

                var isAcronym = char.IsLetter(span[0]);

                if (isAcronym)
                {
                    for (var i = 0; i < length / 2; i++)
                    {
                        if (!(span[(i * 2) + 1] == '.' && char.IsLetter(span[(i * 2) + 2])))
                        {
                            isAcronym = false;
                            break;
                        }
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
