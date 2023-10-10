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
            this.additionalWordCharacters = additionalWordCharacters?.Distinct().OrderBy(x => x).ToArray();
            this.wordConnectingCharacters = wordConnectingCharacters?.Distinct().OrderBy(x => x).ToArray();
            this.numberConnectingCharacters = numberConnectingCharacters?.Distinct().OrderBy(x => x).ToArray();
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
            return other is BasicTokenizer tokenizer
                && SequenceEqual(this.additionalWordCharacters, tokenizer.additionalWordCharacters)
                && SequenceEqual(this.wordConnectingCharacters, tokenizer.wordConnectingCharacters)
                && SequenceEqual(this.numberConnectingCharacters, tokenizer.numberConnectingCharacters);

            static bool SequenceEqual(char[]? a, char[]? b)
            {
                if (a == b)
                {
                    return true;
                }

                if (a is not null && b is not null)
                {
                    return a.AsSpan().SequenceEqual(b.AsSpan());
                }

                return false;
            }
        }
    }
}
