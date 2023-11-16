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
            private readonly TokenMapping mapping = new();
            private readonly Dictionary<string, TokenNode> children = new();
            private readonly ListSlim<string> indexReplacementTokens = new();
            private readonly ListSlim<string> searchReplacementTokens = new();

            private TokenNode()
            {
                this.pathTokens = Array.Empty<string>();
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

            public Dictionary<string, TokenNode> Children
            {
                get
                {
                    return this.children;
                }
            }

            public ListSlim<string> IndexReplacementTokens
            {
                get
                {
                    return this.indexReplacementTokens;
                }
            }

            public ListSlim<string> SearchReplacementTokens
            {
                get
                {
                    return this.searchReplacementTokens;
                }
            }

            public static TokenNode Build(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, bool expand, StringPoolSlim stringPool)
            {
                var root = new TokenNode();
                var tokenTermSource = analyzer.CreateTokenTermSource();
                var tempTokens = new ListSlim<string>();

                foreach (var synonym in synonyms)
                {
                    foreach (var tokens in GetTokens(synonym.Phrases))
                    {
                        var node = GetOrAddNode(root, tokens);

                        node.mapping.AllNodes.Add(node);

                        if (synonym is EquivalencySynonym equivalencySynonym)
                        {
                            var isFirst = true;

                            foreach (var mappedTokens in GetTokens(equivalencySynonym.Phrases))
                            {
                                var mappedNode = GetOrAddNode(root, mappedTokens);

                                node.mapping.AllNodes.Add(mappedNode);

                                if (isFirst || expand)
                                {
                                    node.mapping.MappedNodes.Add(mappedNode);
                                    isFirst = false;
                                }
                            }
                        }
                        else if (synonym is ExplicitMappingSynonym explicitMappingSynonym)
                        {
                            foreach (var mappedTokens in GetTokens(explicitMappingSynonym.MappedPhrases))
                            {
                                var mappedNode = GetOrAddNode(root, mappedTokens);

                                node.mapping.AllNodes.Add(mappedNode);
                                node.mapping.MappedNodes.Add(mappedNode);
                            }
                        }
                    }
                }

                foreach (var node in GetAllNodes(root))
                {
                    foreach (var mappedNode in node.mapping.MappedNodes)
                    {
                        node.indexReplacementTokens.AddRange(mappedNode.pathTokens);
                    }

                    if (!node.mapping.MappedNodes.Contains(node))
                    {
                        foreach (var mappedNode in node.mapping.MappedNodes)
                        {
                            node.searchReplacementTokens.AddRange(mappedNode.pathTokens);
                        }
                    }
                }

                return root;

                IEnumerable<ListSlim<string>> GetTokens(IEnumerable<string> phrases)
                {
                    foreach (var phrase in phrases)
                    {
                        tempTokens.Clear();

                        foreach (var term in tokenTermSource.GetTerms(phrase))
                        {
                            tempTokens.Add(stringPool.GetOrAdd(term.Token));
                        }

                        if (tempTokens.Count > 0)
                        {
                            yield return tempTokens;
                        }
                    }
                }
            }

            private static IEnumerable<TokenNode> GetAllNodes(TokenNode root)
            {
                var queue = new Queue<TokenNode>();

                queue.Enqueue(root);

                while (queue.Count > 0)
                {
                    var node = queue.Dequeue();

                    yield return node;

                    foreach (var child in node.children.Values)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            private static TokenNode GetOrAddNode(TokenNode root, ListSlim<string> tokens)
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

                return node;
            }

            private sealed class TokenMapping
            {
                public HashSet<TokenNode> AllNodes { get; } = new();

                public HashSet<TokenNode> MappedNodes { get; } = new();
            }
        }
    }
}
