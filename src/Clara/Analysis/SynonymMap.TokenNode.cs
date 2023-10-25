using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private readonly string[] pathTokens;
            private readonly string? token;
            private readonly TokenNode? parent;
            private readonly Dictionary<string, TokenNode> children = new();
            private TokenAggregate aggregate;
            private ListSlim<string>? replacementTokens;

            private TokenNode()
            {
                this.pathTokens = Array.Empty<string>();
                this.aggregate = new TokenAggregate(this);
            }

            private TokenNode(string token, TokenNode parent)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (parent is null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }

                var pathTokens = new string[parent.pathTokens.Length + 1];

                Array.Copy(parent.pathTokens, pathTokens, parent.pathTokens.Length);
                pathTokens[pathTokens.Length - 1] = token;

                this.pathTokens = pathTokens;
                this.token = token;
                this.parent = parent;
                this.aggregate = new TokenAggregate(this);
            }

            public string Token
            {
                get
                {
                    if (this.token is null)
                    {
                        throw new InvalidOperationException("Unable to retrieve token value for root token node.");
                    }

                    return this.token;
                }
            }

            public IReadOnlyDictionary<string, TokenNode> Children
            {
                get
                {
                    return this.children;
                }
            }

            public TokenNode Parent
            {
                get
                {
                    if (this.parent is null)
                    {
                        throw new InvalidOperationException("Unable to retrieve parent node value for root token node.");
                    }

                    return this.parent;
                }
            }

            public bool IsRoot
            {
                get
                {
                    return this.parent is null;
                }
            }

            public ListSlim<string> ReplacementTokens
            {
                get
                {
                    if (this.replacementTokens is null)
                    {
                        var replacementTokens = new HashSet<string>();
                        var nodes = Enumerable.Empty<TokenNode>();

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            nodes = this.aggregate.GetRecursiveMappedTo();
                        }
                        else if (this.aggregate.Nodes.Count > 1)
                        {
                            nodes = this.aggregate.Nodes;
                        }

                        foreach (var node in nodes)
                        {
                            foreach (var token in node.pathTokens)
                            {
                                replacementTokens.Add(token);
                            }
                        }

                        this.replacementTokens = new ListSlim<string>(replacementTokens);
                    }

                    return this.replacementTokens;
                }
            }

            public static TokenNode Build(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, StringPoolSlim stringPool)
            {
                var tokenTermSource = analyzer.CreateTokenTermSource();
                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    var aggregate = new TokenAggregate();

                    foreach (var tokens in GetTokens((HashSetSlim<string>)synonym.Phrases))
                    {
                        var node = root;

                        foreach (var token in tokens)
                        {
                            if (!node.children.TryGetValue(token, out var child))
                            {
                                node.children.Add(token, child = new TokenNode(token, node));
                            }

                            node = child;
                        }

                        aggregate.MergeWith(node.aggregate);
                    }

                    if (aggregate.Nodes.Count == 0)
                    {
                        continue;
                    }

                    foreach (var node in aggregate.Nodes)
                    {
                        node.aggregate = aggregate;
                    }

                    if (synonym is MappingSynonym mappingSynonym)
                    {
                        foreach (var tokens in GetTokens((HashSetSlim<string>)mappingSynonym.MappedPhrases))
                        {
                            var node = root;

                            foreach (var token in tokens)
                            {
                                if (!node.children.TryGetValue(token, out var child))
                                {
                                    node.children.Add(token, child = new TokenNode(token, node));
                                }

                                node = child;
                            }

                            aggregate.MapTo(node.aggregate);
                        }
                    }
                }

                return root;

                IEnumerable<ListSlim<string>> GetTokens(HashSetSlim<string> phrases)
                {
                    foreach (var phrase in phrases)
                    {
                        var tokens = new ListSlim<string>();

                        foreach (var term in tokenTermSource.GetTerms(phrase))
                        {
                            tokens.Add(stringPool.GetOrAdd(term.Token));
                        }

                        if (tokens.Count > 0)
                        {
                            yield return tokens;
                        }
                    }
                }
            }

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
