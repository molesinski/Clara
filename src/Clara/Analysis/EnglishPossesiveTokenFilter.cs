namespace Clara.Analysis
{
    public class EnglishPossesiveTokenFilter : ITokenFilter
    {
        public Token Filter(Token token)
        {
            var chars = token.Chars;
            var index = token.Index;
            var length = token.Length;

            if (length >= 2)
            {
                if (chars[length - 1] == 's' || chars[length - 1] == 'S')
                {
                    if (chars[length - 2] == '\'' || chars[length - 2] == '\u2019' || chars[length - 2] == '\uFF07')
                    {
                        return new Token(chars, index, length - 2);
                    }
                }
            }

            return token;
        }
    }
}
