using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class Node
        {
            private readonly ListSlim<string> pathTokens;
            private readonly string? token;
            private readonly Node? parent;
            private readonly TokenMapping mapping = new();
            private readonly Dictionary<string, Node> children = new();
            private ListSlim<string>? replacementTokens;
            private PhraseGroup? replacementPhrases;

            private Node()
            {
                this.pathTokens = new ListSlim<string>();
            }

            private Node(string token, Node parent)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                if (parent is null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }

                var pathTokens = new ListSlim<string>(capacity: parent.pathTokens.Count + 1);

                pathTokens.AddRange(parent.pathTokens);
                pathTokens.Add(token);

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

            public Node Parent
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

            public Dictionary<string, Node> Children
            {
                get
                {
                    return this.children;
                }
            }

            public IReadOnlyList<string> TokenReplacements
            {
                get
                {
                    if (this.replacementTokens is null)
                    {
                        var replacementTokens = new ListSlim<string>();

                        foreach (var mappedNode in this.mapping.MappedNodes)
                        {
                            replacementTokens.AddRange(mappedNode.pathTokens);
                        }

                        this.replacementTokens = replacementTokens;
                    }

                    return this.replacementTokens;
                }
            }

            public PhraseGroup ReplacementPhrases
            {
                get
                {
                    if (this.replacementPhrases is null)
                    {
                        var phrases = new ListSlim<Phrase>();

                        if (!this.mapping.MappedNodes.Contains(this))
                        {
                            foreach (var mappedNode in this.mapping.MappedNodes)
                            {
                                phrases.Add(new Phrase(mappedNode.pathTokens));
                            }
                        }

                        this.replacementPhrases = new PhraseGroup(phrases);
                    }

                    return this.replacementPhrases.Value;
                }
            }

            public static Node BuildTree(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, bool expand, StringPoolSlim stringPool)
            {
                var root = new Node();
                var source = analyzer.CreateTokenTermSource();
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

                return root;

                IEnumerable<ListSlim<string>> GetTokens(IEnumerable<string> phrases)
                {
                    foreach (var phrase in phrases)
                    {
                        tempTokens.Clear();

                        foreach (var term in source.GetTerms(phrase))
                        {
                            tempTokens.Add(stringPool.GetOrAdd(term.Token));
                        }

                        if (tempTokens.Count > 0)
                        {
                            yield return tempTokens;
                        }
                    }
                }

                Node GetOrAddNode(Node root, ListSlim<string> tokens)
                {
                    var node = root;

                    foreach (var token in tokens)
                    {
                        if (!node.children.TryGetValue(token, out var child))
                        {
                            node.children.Add(token, child = new Node(token, node));
                        }

                        node = child;
                    }

                    return node;
                }
            }

            private sealed class TokenMapping
            {
                public HashSet<Node> AllNodes { get; } = new();

                public HashSet<Node> MappedNodes { get; } = new();
            }
        }
    }
}
