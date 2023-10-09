namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymTokenResult
        {
            public SynonymTokenResult(Token token)
            {
                this.Token = token;
                this.Node = null;
            }

            public SynonymTokenResult(TokenNode node)
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
