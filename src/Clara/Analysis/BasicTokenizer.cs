namespace Clara.Analysis
{
    public sealed partial class BasicTokenizer : ITokenizer
    {
        private readonly IEnumerable<Token> emptyEnumerable;

        public BasicTokenizer()
        {
            this.emptyEnumerable = new TokenEnumerable(string.Empty);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By design")]
        public TokenEnumerable GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new TokenEnumerable(text);
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

            return new TokenEnumerable(text);
        }

        public bool Equals(ITokenizer? other)
        {
            return other is BasicTokenizer;
        }
    }
}
