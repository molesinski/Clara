using Clara.Analysis;

namespace Clara.Storage
{
    public readonly record struct SearchTerm
    {
        public SearchTerm(string token, Position position)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.Token = token;
            this.Position = position;
        }

        public SearchTerm(SynonymPhraseCollection phrases, Position position)
        {
            this.Phrases = phrases;
            this.Position = position;
        }

        public string? Token { get; }

        public SynonymPhraseCollection? Phrases { get; }

        public Position Position { get; }
    }
}
