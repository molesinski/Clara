namespace Clara.Analysis
{
    public readonly record struct TokenTerm
    {
        public TokenTerm(Token token, Position position)
        {
            this.Token = token;
            this.Position = position;
        }

        public Token Token { get; }

        public Position Position { get; }
    }
}
