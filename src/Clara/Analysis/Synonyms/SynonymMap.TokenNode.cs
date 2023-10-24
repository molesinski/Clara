using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private readonly string synonymToken = string.Concat("__SYNONYM__", Guid.NewGuid().ToString("N"));
            private readonly string? token;
            private readonly TokenNode? parent;
            private readonly Dictionary<string, TokenNode> children = new();
            private TokenAggregate aggregate;
            private ListSlim<string>? replacementTokens;

            private TokenNode()
            {
                this.aggregate = new(this);
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

                this.token = token;
                this.parent = parent;
                this.aggregate = new(this);
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

            public string? SynonymToken
            {
                get
                {
                    if (this.parent?.parent is not null)
                    {
                        if (this.aggregate.Nodes.Count > 1 || this.aggregate.MappedFrom.Count > 0 || this.aggregate.MappedTo.Count > 0)
                        {
                            return this.synonymToken;
                        }
                    }

                    return null;
                }
            }

            public ListSlim<string> ReplacementTokens
            {
                get
                {
                    if (this.replacementTokens is null)
                    {
                        var replacementTokens = new ListSlim<string>();

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            foreach (var node in this.aggregate.GetRecursiveMappedTo())
                            {
                                var token = node.SynonymToken ?? node.Token;

                                if (!replacementTokens.Contains(token))
                                {
                                    replacementTokens.Add(token);
                                }
                            }
                        }
                        else if (this.aggregate.Nodes.Count > 1)
                        {
                            foreach (var node in this.aggregate.Nodes)
                            {
                                var token = node.SynonymToken ?? node.Token;

                                if (!replacementTokens.Contains(token))
                                {
                                    replacementTokens.Add(token);
                                }
                            }
                        }

                        this.replacementTokens = replacementTokens;
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
        }
    }
}
