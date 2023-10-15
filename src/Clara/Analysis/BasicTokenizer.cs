namespace Clara.Analysis
{
    public sealed partial class BasicTokenizer : ITokenizer
    {
        private static readonly IEnumerable<Token> Empty = new TokenEnumerable(string.Empty);

        public IEnumerable<Token> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Empty;
            }

            return new TokenEnumerable(text);
        }

        public bool Equals(ITokenizer? other)
        {
            return other is BasicTokenizer;
        }
    }
}
