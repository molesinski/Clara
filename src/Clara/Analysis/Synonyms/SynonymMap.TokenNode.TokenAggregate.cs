namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private sealed class TokenAggregate
            {
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

                public HashSet<TokenNode> Nodes
                {
                    get
                    {
                        return this.nodes;
                    }
                }

                public HashSet<TokenNode> MappedFrom
                {
                    get
                    {
                        return this.mappedFrom;
                    }
                }

                public HashSet<TokenNode> MappedTo
                {
                    get
                    {
                        return this.mappedTo;
                    }
                }

                public IReadOnlyCollection<TokenNode> GetRecursiveMappedTo()
                {
                    if (this.mappedTo.Count == 0)
                    {
                        return Array.Empty<TokenNode>();
                    }

                    var result = new HashSet<TokenNode>();
                    var seen = new HashSet<TokenNode>(this.mappedTo);
                    var queue = new Queue<TokenNode>(this.mappedTo);

                    while (queue.Count > 0)
                    {
                        var node = queue.Dequeue();

                        if (node.aggregate.mappedTo.Count > 0)
                        {
                            foreach (var mappedTo in node.aggregate.mappedTo)
                            {
                                if (!seen.Contains(node))
                                {
                                    queue.Enqueue(mappedTo);
                                    seen.Add(mappedTo);
                                }
                                else
                                {
                                    result.Add(mappedTo);
                                }
                            }
                        }
                        else
                        {
                            result.Add(node);
                        }
                    }

                    return result;
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
