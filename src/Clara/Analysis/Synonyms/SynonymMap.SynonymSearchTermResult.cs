namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private readonly struct SynonymSearchTermResult
        {
            public SynonymSearchTermResult(int ordinal, string token)
            {
                this.Ordinal = ordinal;
                this.Token = token;
            }

            public SynonymSearchTermResult(int ordinal, TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Ordinal = ordinal;
                this.Node = node;
            }

            public int Ordinal { get; }

            public string? Token { get; }

            public TokenNode? Node { get; }
        }
    }
}
