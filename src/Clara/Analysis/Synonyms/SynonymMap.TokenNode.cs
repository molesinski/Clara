using Clara.Analysis.MatchExpressions;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap
    {
        private sealed partial class TokenNode
        {
            private static readonly ListSlim<Token> EmptyTokenPath = new();

            private readonly Dictionary<Token, TokenNode> children = new();
            private readonly TokenNode? parent;
            private readonly Token? token;
            private readonly ListSlim<Token> tokenPath = EmptyTokenPath;
            private TokenAggregate aggregate;
            private MatchExpression? matchExpression;
            private ListSlim<Token>? replacementTokens;

            private TokenNode()
            {
                this.aggregate = new(this);
            }

            private TokenNode(TokenNode parent, Token token)
            {
                if (parent is null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }

                if (token.Length == 0)
                {
                    throw new ArgumentException("Token must be not empty.", nameof(token));
                }

                var tokenPath = new ListSlim<Token>(parent.tokenPath);

                tokenPath.Add(token);

                this.parent = parent;
                this.token = token;
                this.tokenPath = tokenPath;
                this.aggregate = new(this);
            }

            public IReadOnlyDictionary<Token, TokenNode> Children
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

            public Token Token
            {
                get
                {
                    if (this.token is null)
                    {
                        throw new InvalidOperationException("Unable to retrieve token value for root token node.");
                    }

                    return this.token.Value;
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
                        var synonymTokens = new HashSetSlim<Token>();

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

                        if (synonymTokens.Count > 0)
                        {
                            expressions.Add(Match.Any(ScoreAggregation.Max, synonymTokens));
                        }

                        if (expressions.Count == 1)
                        {
                            this.matchExpression = expressions[0].ToPersistent();
                        }
                        else
                        {
                            this.matchExpression = Match.Or(ScoreAggregation.Max, expressions).ToPersistent();
                        }
                    }

                    return this.matchExpression;
                }
            }

            public ListSlim<Token> ReplacementTokens
            {
                get
                {
                    if (this.replacementTokens is null)
                    {
                        var replacementTokens = new ListSlim<Token>();
                        var synonymTokens = new HashSetSlim<Token>();

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

            public static TokenNode Build(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold, out DictionarySlim<Token, string> tokenMap)
            {
                tokenMap = new DictionarySlim<Token, string>();

                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    var aggregate = new TokenAggregate();

                    foreach (var tokens in GetTokenPermutations((HashSetSlim<string>)synonym.Phrases, tokenMap))
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

                    if (synonym is ExplicitMappingSynonym explicitMappingSynonym)
                    {
                        foreach (var tokens in GetTokenPermutations((HashSetSlim<string>)explicitMappingSynonym.MappedPhrases, tokenMap))
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

                IEnumerable<ListSlim<Token>> GetTokenPermutations(HashSetSlim<string> phrases, DictionarySlim<Token, string> tokenMap)
                {
                    foreach (var phrase in phrases)
                    {
                        var tokens = new ListSlim<Token>();

                        foreach (var token in analyzer.GetTokens(phrase))
                        {
                            if (!tokenMap.TryGetValue(token, out var value))
                            {
                                value = token.ToString();

                                tokenMap[new Token(value)] = value;
                            }

                            tokens.Add(new Token(value));
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
