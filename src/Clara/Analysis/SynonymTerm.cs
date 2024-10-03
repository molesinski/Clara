namespace Clara.Analysis
{
    public readonly record struct SynonymTerm
    {
        public SynonymTerm(Token token, Position position)
        {
            this.Token = token;
            this.Position = position;
        }

        public SynonymTerm(SynonymPhraseCollection phrases, Position position)
        {
            this.Phrases = phrases;
            this.Position = position;
        }

        public Token? Token { get; }

        public SynonymPhraseCollection? Phrases { get; }

        public Position Position { get; }
    }
}
