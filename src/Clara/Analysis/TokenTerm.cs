namespace Clara.Analysis
{
    public readonly record struct TokenTerm
    {
        public TokenTerm(Token token, int position)
        {
            this.Token = token;
            this.Position = position;
        }

        public Token Token { get; }

        public int Position { get; }
    }
}
