using Clara.Analysis.MatchExpressions;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private static readonly ListSlim<string> EmptyTokenPath = new();

            private readonly Dictionary<string, TokenNode> children = new();
            private readonly TokenNode? parent;
            private readonly string? token;
            private readonly ListSlim<string> tokenPath = EmptyTokenPath;
            private TokenAggregate aggregate;
            private MatchExpression? matchExpression;
            private ListSlim<string>? replacementTokens;

            private TokenNode()
            {
                this.aggregate = new(this);
            }

            private TokenNode(TokenNode parent, string token)
            {
                if (parent is null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }

                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                var tokenPath = new ListSlim<string>(parent.tokenPath);

                tokenPath.Add(token);

                this.parent = parent;
                this.token = token;
                this.tokenPath = tokenPath;
                this.aggregate = new(this);
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

            public bool IsRoot
            {
                get
                {
                    return this.parent is null;
                }
            }

            public bool HasSynonyms
            {
                get
                {
                    return this.aggregate.Nodes.Count > 1
                        || this.aggregate.MappedTo.Count > 0
                        || this.aggregate.MappedFrom.Count > 0;
                }
            }

            public MatchExpression MatchExpression
            {
                get
                {
                    if (this.matchExpression is null)
                    {
                        var expressions = new ListSlim<MatchExpression>();
                        var synonymTokens = new HashSetSlim<string>();

                        if (this.aggregate.MappedFrom.Count > 0 || this.aggregate.Nodes.Count > 1)
                        {
                            synonymTokens.Add(this.aggregate.SynonymToken);
                        }

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            foreach (var node in this.aggregate.MappedTo)
                            {
                                synonymTokens.Add(node.aggregate.SynonymToken);
                            }
                        }
                        else
                        {
                            expressions.Add(Match.All(ScoreAggregation.Sum, this.tokenPath));
                        }

                        if (synonymTokens.Count > 0)
                        {
                            expressions.Insert(0, Match.Any(ScoreAggregation.Max, synonymTokens, isLazy: false));
                        }

                        if (expressions.Count == 1)
                        {
                            this.matchExpression = expressions[0];
                        }
                        else
                        {
                            this.matchExpression = Match.Or(ScoreAggregation.Max, expressions, isLazy: true);
                        }
                    }

                    return this.matchExpression;
                }
            }

            public ListSlim<string> ReplacementTokens
            {
                get
                {
                    if (this.replacementTokens is null)
                    {
                        var replacementTokens = new ListSlim<string>();
                        var synonymTokens = new HashSetSlim<string>();

                        if (this.aggregate.MappedFrom.Count > 0 || this.aggregate.Nodes.Count > 1)
                        {
                            synonymTokens.Add(this.aggregate.SynonymToken);
                        }

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            foreach (var node in this.aggregate.MappedTo)
                            {
                                synonymTokens.Add(node.aggregate.SynonymToken);

                                replacementTokens.AddRange(node.tokenPath);
                            }
                        }
                        else
                        {
                            replacementTokens.AddRange(this.tokenPath);
                        }

                        if (synonymTokens.Count > 0)
                        {
                            replacementTokens.AddRange(synonymTokens);
                        }

                        this.replacementTokens = replacementTokens;
                    }

                    return this.replacementTokens;
                }
            }

            public static TokenNode Build(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold, StringPoolSlim stringPool)
            {
                var tokenTermSource = analyzer.CreateTokenTermSource();
                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    var aggregate = new TokenAggregate();

                    foreach (var tokens in GetTokenPermutations((HashSetSlim<string>)synonym.Phrases, stringPool))
                    {
                        var node = root;

                        foreach (var token in tokens)
                        {
                            if (!node.children.TryGetValue(token, out var child))
                            {
                                node.children.Add(token, child = new TokenNode(node, token));
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
                        foreach (var tokens in GetTokenPermutations((HashSetSlim<string>)mappingSynonym.MappedPhrases, stringPool))
                        {
                            var node = root;

                            foreach (var token in tokens)
                            {
                                if (!node.children.TryGetValue(token, out var child))
                                {
                                    node.children.Add(token, child = new TokenNode(node, token));
                                }

                                node = child;
                            }

                            aggregate.MapTo(node.aggregate);
                        }
                    }
                }

                return root;

                IEnumerable<ListSlim<string>> GetTokenPermutations(HashSetSlim<string> phrases, StringPoolSlim stringPool)
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
                            if (tokens.Count > 1 && tokens.Count <= permutatedTokenCountThreshold)
                            {
                                foreach (var tokenPermutation in PermutationHelper.Permutate(tokens))
                                {
                                    yield return tokenPermutation;
                                }
                            }
                            else
                            {
                                yield return tokens;
                            }
                        }
                    }
                }
            }
        }
    }
}
