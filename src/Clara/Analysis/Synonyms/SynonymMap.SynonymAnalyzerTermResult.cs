namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymAnalyzerTermResult
        {
            public SynonymAnalyzerTermResult(int position, Token token)
            {
                this.Position = position;
                this.Token = token;
            }

            public SynonymAnalyzerTermResult(int position, TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Position = position;
                this.Node = node;
            }

            public int Position { get; }

            public Token? Token { get; }

            public TokenNode? Node { get; }
        }
    }
}
