namespace Clara.Analysis
{
    public readonly record struct AnalyzerTerm
    {
        public AnalyzerTerm(int ordinal, Token token)
        {
            this.Ordinal = ordinal;
            this.Token = token;
        }

        public int Ordinal { get; }

        public Token Token { get; }
    }
}
