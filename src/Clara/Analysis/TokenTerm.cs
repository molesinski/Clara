namespace Clara.Analysis
{
    public readonly record struct TokenTerm
    {
        public TokenTerm(Token token, Offset offset)
        {
            this.Token = token;
            this.Offset = offset;
        }

        public Token Token { get; }

        public Offset Offset { get; }
    }
}
