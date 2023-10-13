namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymSearchTermResult
        {
            public SynonymSearchTermResult(int position, string token)
            {
                this.Position = position;
                this.Token = token;
            }

            public SynonymSearchTermResult(int position, TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Position = position;
                this.Node = node;
            }

            public int Position { get; }

            public string? Token { get; }

            public TokenNode? Node { get; }
        }
    }
}
