using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private readonly string[] pathTokens;
            private readonly bool expand;
            private readonly string? token;
            private readonly TokenNode? parent;
            private readonly Dictionary<string, TokenNode> children = new();
            private TokenAggregate aggregate;
            private ListSlim<string>? indexReplacementTokens;
            private ListSlim<string>? searchReplacementTokens;

            private TokenNode()
            {
                this.pathTokens = Array.Empty<string>();
                this.aggregate = new TokenAggregate(this);
            }

            private TokenNode(bool expand, string token, TokenNode parent)
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
                this.expand = expand;
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

            public ListSlim<string> IndexReplacementTokens
            {
                get
                {
                    if (this.indexReplacementTokens is null)
                    {
                        var tokens = new ListSlim<string>();

                        if (this.aggregate.AllNodes.Count > 1)
                        {
                            foreach (var node in this.aggregate.MappedNodes)
                            {
                                tokens.AddRange(node.pathTokens);

                                if (!this.expand)
                                {
                                    break;
                                }
                            }
                        }

                        this.indexReplacementTokens = tokens;
                    }

                    return this.indexReplacementTokens;
                }
            }

            public ListSlim<string> SearchReplacementTokens
            {
                get
                {
                    if (this.searchReplacementTokens is null)
                    {
                        var tokens = new ListSlim<string>();

                        if (this.aggregate.AllNodes.Count > 1)
                        {
                            var replace = false;

                            if (this.expand)
                            {
                                replace = !this.aggregate.MappedNodes.Contains(this);
                            }
                            else
                            {
                                foreach (var node in this.aggregate.MappedNodes)
                                {
                                    replace = this != node;
                                    break;
                                }
                            }

                            if (replace)
                            {
                                foreach (var node in this.aggregate.MappedNodes)
                                {
                                    tokens.AddRange(node.pathTokens);

                                    if (!this.expand)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        this.searchReplacementTokens = tokens;
                    }

                    return this.searchReplacementTokens;
                }
            }

            public static TokenNode Build(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, bool expand, StringPoolSlim stringPool)
            {
                var tokenTermSource = analyzer.CreateTokenTermSource();
                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    var aggregate = new TokenAggregate();

                    foreach (var tokens in GetTokens(synonym.Phrases))
                    {
                        var node = root;

                        foreach (var token in tokens)
                        {
                            if (!node.children.TryGetValue(token, out var child))
                            {
                                node.children.Add(token, child = new TokenNode(expand, token, node));
                            }

                            node = child;
                        }

                        aggregate.AllNodes.UnionWith(node.aggregate.AllNodes);
                        aggregate.MappedNodes.UnionWith(node.aggregate.MappedNodes);
                    }

                    if (aggregate.AllNodes.Count == 0)
                    {
                        continue;
                    }

                    foreach (var node in aggregate.AllNodes)
                    {
                        node.aggregate = aggregate;
                    }

                    if (synonym is MappingSynonym mappingSynonym)
                    {
                        aggregate.MappedNodes.Clear();

                        foreach (var tokens in GetTokens(mappingSynonym.MappedPhrases))
                        {
                            var node = root;

                            foreach (var token in tokens)
                            {
                                if (!node.children.TryGetValue(token, out var child))
                                {
                                    node.children.Add(token, child = new TokenNode(expand, token, node));
                                }

                                node = child;
                            }

                            aggregate.AllNodes.Add(node);
                            aggregate.MappedNodes.Add(node);
                            node.aggregate = aggregate;
                        }
                    }
                }

                return root;

                IEnumerable<ListSlim<string>> GetTokens(IEnumerable<string> phrases)
                {
                    var tokens = new ListSlim<string>();

                    foreach (var phrase in phrases)
                    {
                        tokens.Clear();

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
                public TokenAggregate()
                {
                }

                public TokenAggregate(TokenNode node)
                {
                    this.AllNodes.Add(node);
                    this.MappedNodes.Add(node);
                }

                public HashSet<TokenNode> AllNodes { get; } = new HashSet<TokenNode>();

                public HashSet<TokenNode> MappedNodes { get; } = new HashSet<TokenNode>();
            }
        }
    }
}
