namespace Clara.Analysis
{
    public sealed partial class BasicTokenizer : ITokenizer
    {
        private readonly IEnumerable<Token> emptyEnumerable;
        private readonly char[]? additionalWordCharacters;
        private readonly char[]? wordConnectingCharacters;
        private readonly char[]? numberConnectingCharacters;

        public BasicTokenizer(
            IEnumerable<char>? additionalWordCharacters = null,
            IEnumerable<char>? wordConnectingCharacters = null,
            IEnumerable<char>? numberConnectingCharacters = null)
        {
            this.emptyEnumerable = new TokenEnumerable(this, string.Empty);
            this.additionalWordCharacters = additionalWordCharacters?.ToArray();
            this.wordConnectingCharacters = wordConnectingCharacters?.ToArray();
            this.numberConnectingCharacters = numberConnectingCharacters?.ToArray();
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
    }
}
