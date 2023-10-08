namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymResult
        {
            public SynonymResult(Token token)
            {
                this.Token = token;
                this.Node = null;
            }

            public SynonymResult(TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Token = null;
                this.Node = node;
            }

            public Token? Token { get; }

            public TokenNode? Node { get; }
        }
    }
}
