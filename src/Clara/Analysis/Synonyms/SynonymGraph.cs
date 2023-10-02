using System.Collections;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed class SynonymGraph : ISynonymMap
    {
        private readonly IAnalyzer analyzer;
        private readonly TokenNode root;

        public SynonymGraph(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold = 1)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (synonyms is null)
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            if (permutatedTokenCountThreshold < 1 || permutatedTokenCountThreshold > PermutationHelper.MaximumPermutatedTokenCount)
            {
                throw new ArgumentOutOfRangeException(nameof(permutatedTokenCountThreshold));
            }

            this.analyzer = analyzer;
            this.root = TokenNode.BuildGraph(analyzer, synonyms, permutatedTokenCountThreshold);
        }

        public IAnalyzer Analyzer
        {
            get
            {
                return this.analyzer;
            }
        }

        private bool IsEmpty
        {
            get
            {
                return this.root.Children.Count == 0;
            }
        }

        public IEnumerable<string> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (this.IsEmpty)
            {
                return this.analyzer.GetTokens(text);
            }

            return GetTokensEnumerable(text);

            IEnumerable<string> GetTokensEnumerable(string text)
            {
                foreach (var item in new SynonymResultEnumerable(this.root, this.analyzer.GetTokens(text)))
                {
                    if (item.Node is TokenNode node)
                    {
                        foreach (var token in node.ReplacementTokens)
                        {
                            yield return token;
                        }
                    }
                    else if (item.Token is string token)
                    {
                        yield return token;
                    }
                }
            }
        }

        public MatchExpression Process(MatchExpression matchExpression)
        {
            if (matchExpression is null)
            {
                throw new ArgumentNullException(nameof(matchExpression));
            }

            if (this.IsEmpty)
            {
                return matchExpression;
            }

            if (matchExpression is AllTokensMatchExpression allValuesMatchExpression)
            {
                var expressions = default(ListSlim<MatchExpression>?);
                var tokens = default(ListSlim<string>?);

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, allValuesMatchExpression.Tokens))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions ??= new();
                        expressions.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is string token)
                    {
                        tokens ??= new();
                        tokens.Add(token);
                    }
                }

                if (expressions is null)
                {
                    return matchExpression;
                }

                if (tokens is not null)
                {
                    expressions.Insert(0, new AllTokensMatchExpression(tokens));
                }

                if (expressions.Count == 1)
                {
                    return expressions[0];
                }
                else
                {
                    return new AndMatchExpression(expressions);
                }
            }
            else if (matchExpression is AnyTokensMatchExpression anyValuesMatchExpression)
            {
                var expressions = default(ListSlim<MatchExpression>?);
                var tokens = default(ListSlim<string>?);

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, anyValuesMatchExpression.Tokens))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions ??= new();
                        expressions.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is string token)
                    {
                        tokens ??= new();
                        tokens.Add(token);
                    }
                }

                if (expressions is null)
                {
                    return matchExpression;
                }

                if (tokens is not null)
                {
                    expressions.Insert(0, new AnyTokensMatchExpression(tokens));
                }

                if (expressions.Count == 1)
                {
                    return expressions[0];
                }
                else
                {
                    return new OrMatchExpression(expressions);
                }
            }
            else
            {
                return matchExpression;
            }
        }

        private readonly struct SynonymResultEnumerable : IEnumerable<SynonymResult>
        {
            private readonly TokenNode root;
            private readonly IEnumerable<string> tokens;

            public SynonymResultEnumerable(TokenNode root, IEnumerable<string> tokens)
            {
                if (root is null)
                {
                    throw new ArgumentNullException(nameof(root));
                }

                if (tokens is null)
                {
                    throw new ArgumentNullException(nameof(tokens));
                }

                this.root = root;
                this.tokens = tokens;
            }

            public readonly Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            readonly IEnumerator<SynonymResult> IEnumerable<SynonymResult>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            readonly IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public struct Enumerator : IEnumerator<SynonymResult>
            {
                private readonly TokenNode root;
                private readonly IEnumerable<string> tokens;
                private TokenNode currentNode;
                private TokenNode? backtrackingNode;
                private IEnumerator<string>? enumerator;
                private string? previousToken;
                private bool isEnumerated;
                private SynonymResult current;

                public Enumerator(SynonymResultEnumerable source)
                {
                    this.root = source.root;
                    this.tokens = source.tokens;
                    this.currentNode = this.root;
                    this.backtrackingNode = null;
                    this.previousToken = null;
                    this.enumerator = null;
                    this.isEnumerated = false;
                    this.current = default;
                }

                public readonly SynonymResult Current
                {
                    get
                    {
                        return this.current;
                    }
                }

                readonly object IEnumerator.Current
                {
                    get
                    {
                        return this.current;
                    }
                }

                public bool MoveNext()
                {
                    if (this.isEnumerated)
                    {
                        return false;
                    }

                    this.enumerator ??= this.tokens.GetEnumerator();

                    while (this.backtrackingNode is not null || !this.isEnumerated)
                    {
                        while (this.backtrackingNode is not null)
                        {
                            if (this.backtrackingNode.IsRoot)
                            {
                                this.backtrackingNode = null;

                                break;
                            }

                            if (!this.backtrackingNode.IsEmpty)
                            {
                                this.current = new SynonymResult(this.backtrackingNode);
                                this.backtrackingNode = null;

                                return true;
                            }

                            this.current = new SynonymResult(this.backtrackingNode.Token);
                            this.backtrackingNode = this.backtrackingNode.Parent;

                            return true;
                        }

                        if (this.previousToken is not null || (!this.isEnumerated && this.enumerator.MoveNext()))
                        {
                            var currentToken = this.previousToken ?? this.enumerator.Current;

                            this.previousToken = null;

                            if (this.currentNode.Children.TryGetValue(currentToken, out var node))
                            {
                                this.currentNode = node;
                                continue;
                            }

                            if (!this.currentNode.IsRoot)
                            {
                                this.backtrackingNode = this.currentNode;
                                this.currentNode = this.root;
                                this.previousToken = currentToken;

                                continue;
                            }

                            this.current = new SynonymResult(currentToken);

                            return true;
                        }
                        else
                        {
                            if (!this.isEnumerated)
                            {
                                this.isEnumerated = true;

                                if (!this.currentNode.IsRoot)
                                {
                                    this.backtrackingNode = this.currentNode;
                                    this.currentNode = this.root;
                                    this.previousToken = null;

                                    continue;
                                }
                            }
                        }
                    }

                    this.isEnumerated = true;
                    this.current = default;

                    return false;
                }

                public void Reset()
                {
                    this.currentNode = this.root;
                    this.backtrackingNode = null;
                    this.previousToken = null;
                    this.enumerator?.Dispose();
                    this.enumerator = null;
                    this.isEnumerated = false;
                    this.current = default;
                }

                public void Dispose()
                {
                    this.enumerator?.Dispose();
                    this.enumerator = null;
                }
            }
        }

        private readonly struct SynonymResult
        {
            public SynonymResult(string token)
            {
                if (token is null)
                {
                    throw new ArgumentNullException(nameof(token));
                }

                this.Token = token;
                this.Node = null;
            }

            public SynonymResult(TokenNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Token = null;
                this.Node = node;
            }

            public string? Token { get; }

            public TokenNode? Node { get; }
        }

        private sealed class TokenNode
        {
            private readonly TokenNode? parent;
            private readonly string? token;
            private readonly string[] tokenPath;
            private readonly Dictionary<string, TokenNode> children;
            private TokenAggregate aggregate;
            private MatchExpression? matchExpression;
            private ListSlim<string>? replacementTokens;

            private TokenNode()
            {
                this.parent = null;
                this.token = null;
                this.tokenPath = Array.Empty<string>();
                this.children = new();
                this.aggregate = new(this);
            }

            private TokenNode(TokenNode parent, string token)
                : this()
            {
                if (parent is null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }

                var length = parent.tokenPath.Length;
                var pathTokens = new string[length + 1];

                Array.Copy(parent.tokenPath, pathTokens, length);

                pathTokens[length] = token;

                this.parent = parent;
                this.token = token;
                this.tokenPath = pathTokens;
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

            public Dictionary<string, TokenNode> Children
            {
                get
                {
                    return this.children;
                }
            }

            public bool IsRoot
            {
                get
                {
                    return this.parent is null;
                }
            }

            public bool IsEmpty
            {
                get
                {
                    return this.aggregate.Nodes.Count <= 1
                        && this.aggregate.MappedTo.Count == 0
                        && this.aggregate.MappedFrom.Count == 0;
                }
            }

            public MatchExpression MatchExpression
            {
                get
                {
                    if (this.matchExpression is null)
                    {
                        var expressions = new ListSlim<MatchExpression>();

                        expressions.Add(Match.All(this.aggregate.SynonymToken));

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            foreach (var node in this.aggregate.MappedTo)
                            {
                                expressions.Add(Match.All(node.aggregate.SynonymToken));
                            }
                        }
                        else
                        {
                            expressions.Add(Match.All(this.tokenPath));
                        }

                        this.matchExpression = Match.Or(expressions);
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

                        replacementTokens.Add(this.aggregate.SynonymToken);

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            foreach (var node in this.aggregate.MappedTo)
                            {
                                replacementTokens.Add(node.aggregate.SynonymToken);
                                replacementTokens.AddRange(node.tokenPath);
                            }
                        }
                        else
                        {
                            replacementTokens.AddRange(this.tokenPath);
                        }

                        this.replacementTokens = replacementTokens;
                    }

                    return this.replacementTokens;
                }
            }

            public static TokenNode BuildGraph(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold)
            {
                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    if (synonym is EquivalencySynonym equivalencySynonym)
                    {
                        var aggregate = new TokenAggregate();

                        foreach (var tokens in GetTokenPermutations(equivalencySynonym.Phrases))
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

                            aggregate.UnionWith(node.aggregate);
                        }

                        foreach (var node in aggregate.Nodes)
                        {
                            node.aggregate = aggregate;
                        }
                    }
                    else if (synonym is ExplicitMappingSynonym explicitMappingSynonym)
                    {
                        var mappedTo = new HashSet<TokenNode>();

                        foreach (var tokens in GetTokenPermutations(explicitMappingSynonym.MappedPhrases))
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

                            mappedTo.Add(node);
                        }

                        foreach (var tokens in GetTokenPermutations(explicitMappingSynonym.Phrases))
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

                            foreach (var mappedNode in mappedTo)
                            {
                                node.aggregate.MapTo(mappedNode.aggregate);
                                mappedNode.aggregate.MapFrom(node.aggregate);
                            }
                        }
                    }
                }

                return root;

                IEnumerable<string[]> GetTokenPermutations(IEnumerable<string> phrases)
                {
                    foreach (var phrase in phrases)
                    {
                        var tokens = analyzer.GetTokens(phrase).ToArray();

                        if (permutatedTokenCountThreshold > 1 && tokens.Length <= permutatedTokenCountThreshold)
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

            private sealed class TokenAggregate
            {
                private readonly string synonymToken;
                private readonly HashSet<TokenNode> nodes;
                private readonly HashSet<TokenNode> mappedTo;
                private readonly HashSet<TokenNode> mappedFrom;

                public TokenAggregate()
                {
                    this.synonymToken = string.Concat("__$Synonym$<", Guid.NewGuid().ToString("N"), ">");
                    this.nodes = new();
                    this.mappedTo = new();
                    this.mappedFrom = new();
                }

                public TokenAggregate(TokenNode node)
                    : this()
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

                public void UnionWith(TokenAggregate aggregate)
                {
                    this.nodes.UnionWith(aggregate.nodes);
                    this.mappedFrom.UnionWith(aggregate.mappedFrom);
                    this.mappedTo.UnionWith(aggregate.mappedTo);
                }

                public void MapTo(TokenAggregate aggregate)
                {
                    this.mappedTo.UnionWith(aggregate.nodes);
                    this.mappedTo.UnionWith(aggregate.mappedTo);

                    foreach (var node in this.mappedFrom)
                    {
                        node.aggregate.mappedTo.UnionWith(aggregate.nodes);
                        node.aggregate.mappedTo.UnionWith(aggregate.mappedTo);
                    }
                }

                public void MapFrom(TokenAggregate aggregate)
                {
                    this.mappedFrom.UnionWith(aggregate.nodes);
                    this.mappedFrom.UnionWith(aggregate.mappedFrom);

                    foreach (var node in this.mappedTo)
                    {
                        node.aggregate.mappedFrom.UnionWith(aggregate.nodes);
                        node.aggregate.mappedFrom.UnionWith(aggregate.mappedFrom);
                    }
                }
            }
        }
    }
}
