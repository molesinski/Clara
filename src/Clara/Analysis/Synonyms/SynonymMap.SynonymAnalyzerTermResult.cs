namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymAnalyzerTermResult
        {
            public SynonymAnalyzerTermResult(int ordinal, Token token)
            {
                this.Ordinal = ordinal;
                this.Token = token;
            }

            public SynonymAnalyzerTermResult(int ordinal, TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Ordinal = ordinal;
                this.Node = node;
            }

            public int Ordinal { get; }

            public Token? Token { get; }

            public TokenNode? Node { get; }
        }
    }
}
