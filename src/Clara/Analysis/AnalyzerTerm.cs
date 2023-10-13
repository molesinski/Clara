namespace Clara.Analysis
{
    public readonly record struct AnalyzerTerm
    {
        public AnalyzerTerm(int position, Token token)
        {
            this.Position = position;
            this.Token = token;
        }

        public int Position { get; }

        public Token Token { get; }
    }
}
