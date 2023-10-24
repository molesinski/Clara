namespace Clara.Analysis
{
    public readonly record struct SearchTerm
    {
        public SearchTerm(string token, TokenPosition position)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.Token = token;
            this.Position = position;
        }

        public string Token { get; }

        public TokenPosition Position { get; }
    }
}
