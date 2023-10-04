using System.Collections;
using Clara.Analysis.MatchExpressions;
using Clara.Storage;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed class SynonymMap : ISynonymMap
    {
        public const int MaximumPermutatedTokenCount = 5;

        private readonly IAnalyzer analyzer;
        private readonly HashSet<Synonym> synonyms = new();
        private readonly TokenNode root;

        public SynonymMap(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold = 1)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (synonyms is null)
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            foreach (var synonym in synonyms)
            {
                if (synonym is null)
                {
                    throw new ArgumentException("Synonyms must not be null.", nameof(synonyms));
                }

                this.synonyms.Add(synonym);
            }

            if (permutatedTokenCountThreshold < 1 || permutatedTokenCountThreshold > MaximumPermutatedTokenCount)
            {
                throw new ArgumentOutOfRangeException(nameof(permutatedTokenCountThreshold));
            }

            this.analyzer = analyzer;
            this.root = TokenNode.Build(analyzer, this.synonyms, permutatedTokenCountThreshold);
        }

        public IAnalyzer Analyzer
        {
            get
            {
                return this.analyzer;
            }
        }

        public IReadOnlyCollection<Synonym> Synonyms
        {
            get
            {
                return this.synonyms;
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
                using var tokens = SharedObjectPools.Tokens.Lease();

                var expressions = default(ListSlim<MatchExpression>?);

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, allValuesMatchExpression.Tokens))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions ??= new();
                        expressions.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is string token)
                    {
                        tokens.Instance.Add(token);
                    }
                }

                if (expressions is null)
                {
                    return allValuesMatchExpression;
                }

                if (tokens.Instance.Count > 0)
                {
                    expressions.Insert(0, new AllTokensMatchExpression(allValuesMatchExpression.ScoringMode, new ListSlim<string>(tokens.Instance)));
                }

                if (expressions.Count == 1)
                {
                    return expressions[0];
                }
                else
                {
                    return new AndMatchExpression(allValuesMatchExpression.ScoringMode, expressions);
                }
            }
            else if (matchExpression is AnyTokensMatchExpression anyValuesMatchExpression)
            {
                using var tokens = SharedObjectPools.Tokens.Lease();

                var expressions = default(ListSlim<MatchExpression>?);

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, anyValuesMatchExpression.Tokens))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions ??= new();
                        expressions.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is string token)
                    {
                        tokens.Instance.Add(token);
                    }
                }

                if (expressions is null)
                {
                    return anyValuesMatchExpression;
                }

                if (tokens.Instance.Count > 0)
                {
                    expressions.Insert(0, new AnyTokensMatchExpression(anyValuesMatchExpression.ScoringMode, new ListSlim<string>(tokens.Instance)));
                }

                if (expressions.Count == 1)
                {
                    return expressions[0];
                }
                else
                {
                    return new OrMatchExpression(anyValuesMatchExpression.ScoringMode, expressions);
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

                            if (this.backtrackingNode.HasSynonyms)
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
            private static readonly ListSlim<string> EmptyTokenPath = new();

            private readonly Dictionary<string, TokenNode> children = new();
            private readonly TokenNode? parent;
            private readonly string? token;
            private readonly ListSlim<string> tokenPath = EmptyTokenPath;
            private TokenNodeAggregate aggregate;
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
                            expressions.Add(Match.All(ScoringMode.Sum, this.tokenPath));
                        }

                        if (synonymTokens.Count > 0)
                        {
                            expressions.Add(Match.Any(ScoringMode.Max, synonymTokens));
                        }

                        if (expressions.Count == 1)
                        {
                            this.matchExpression = expressions[0];
                        }
                        else
                        {
                            this.matchExpression = new OrMatchExpression(ScoringMode.Max, expressions);
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

                        replacementTokens.AddRange(synonymTokens);

                        this.replacementTokens = replacementTokens;
                    }

                    return this.replacementTokens;
                }
            }

            public static TokenNode Build(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold)
            {
                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    var aggregate = new TokenNodeAggregate();

                    foreach (var tokens in GetTokenPermutations(synonym.Phrases))
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

                            aggregate.MapTo(node.aggregate);
                        }
                    }
                }

                return root;

                IEnumerable<IEnumerable<string>> GetTokenPermutations(IEnumerable<string> phrases)
                {
                    foreach (var phrase in phrases)
                    {
                        var tokens = analyzer.GetTokens(phrase).ToList();

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

            private sealed class TokenNodeAggregate
            {
                private readonly string synonymToken = string.Concat("__SYNONYM__", Guid.NewGuid().ToString("N"));
                private readonly HashSet<TokenNode> nodes = new();
                private readonly HashSet<TokenNode> mappedTo = new();
                private readonly HashSet<TokenNode> mappedFrom = new();

                public TokenNodeAggregate()
                {
                }

                public TokenNodeAggregate(TokenNode node)
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

                public void MergeWith(TokenNodeAggregate aggregate)
                {
                    this.nodes.UnionWith(aggregate.nodes);
                    this.mappedFrom.UnionWith(aggregate.mappedFrom);
                    this.mappedTo.UnionWith(aggregate.mappedTo);
                }

                public void MapTo(TokenNodeAggregate aggregate)
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
