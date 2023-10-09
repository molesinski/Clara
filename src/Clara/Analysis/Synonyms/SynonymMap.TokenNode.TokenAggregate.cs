namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private sealed class TokenAggregate
            {
                private readonly string synonymToken = string.Concat("__SYNONYM__", Guid.NewGuid().ToString("N"));
                private readonly HashSet<TokenNode> nodes = new();
                private readonly HashSet<TokenNode> mappedTo = new();
                private readonly HashSet<TokenNode> mappedFrom = new();

                public TokenAggregate()
                {
                }

                public TokenAggregate(TokenNode node)
                {
                    if (node is null)
                    {
                        throw new ArgumentNullException(nameof(node));
                    }

                    this.nodes.Add(node);
                }

                public string SynonymToken
                {
                    get
                    {
                        return this.synonymToken;
                    }
                }

                public HashSet<TokenNode> Nodes
                {
                    get
                    {
                        return this.nodes;
                    }
                }

                public HashSet<TokenNode> MappedTo
                {
                    get
                    {
                        return this.mappedTo;
                    }
                }

                public HashSet<TokenNode> MappedFrom
                {
                    get
                    {
                        return this.mappedFrom;
                    }
                }

                public void MergeWith(TokenAggregate aggregate)
                {
                    this.nodes.UnionWith(aggregate.nodes);
                    this.mappedFrom.UnionWith(aggregate.mappedFrom);
                    this.mappedTo.UnionWith(aggregate.mappedTo);
                }

                public void MapTo(TokenAggregate aggregate)
                {
                    this.mappedTo.UnionWith(aggregate.nodes);
                    this.mappedTo.UnionWith(aggregate.mappedTo);

                    aggregate.mappedFrom.UnionWith(this.nodes);
                    aggregate.mappedFrom.UnionWith(this.mappedFrom);

                    foreach (var node in this.mappedFrom)
                    {
                        node.aggregate.mappedTo.UnionWith(aggregate.nodes);
                        node.aggregate.mappedTo.UnionWith(aggregate.mappedTo);
                    }

                    foreach (var node in aggregate.mappedTo)
                    {
                        node.aggregate.mappedFrom.UnionWith(this.nodes);
                        node.aggregate.mappedFrom.UnionWith(this.mappedFrom);
                    }
                }
            }
        }
    }
}
