namespace Clara.Analysis
{
    public sealed class LowerInvariantTokenFilter : ITokenFilter
    {
        public Token Process(Token token)
        {
            var length = token.Length;

            for (var i = 0; i < length; i++)
            {
                token[i] = char.ToLowerInvariant(token[i]);
            }

            return token;
        }
    }
}
