namespace Clara.Analysis
{
    public sealed partial class BasicTokenizer : ITokenizer
    {
        private readonly IEnumerable<Token> emptyEnumerable;

        public BasicTokenizer()
        {
            this.emptyEnumerable = new TokenEnumerable(this, string.Empty);
        }

        public TokenEnumerable GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new TokenEnumerable(this, text);
        }

        IEnumerable<Token> ITokenizer.GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return this.emptyEnumerable;
            }

            return new TokenEnumerable(this, text);
        }

        public bool Equals(ITokenizer? other)
        {
            return other is BasicTokenizer;
        }
    }
}
