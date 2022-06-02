namespace Clara.Analysis
{
    public sealed class LowerInvariantTokenFilter : ITokenFilter
    {
        public Token Filter(Token token)
        {
            var chars = token.Chars;
            var index = token.Index;
            var length = token.Length;

            for (var i = index; i < length; i++)
            {
                chars[i] = char.ToLowerInvariant(chars[i]);
            }

            return token;
        }
    }
}
