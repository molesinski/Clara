using System.Collections;
using Clara.Analysis.MatchExpressions;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed class SynonymMap : ISynonymMap
    {
        public const int MaximumPermutatedTokenCount = 5;

        private readonly ObjectPool<TokenEnumerable> enumerablePool;
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

            this.enumerablePool = new(() => new(this));
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

        public IDisposableEnumerable<string> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return DisposableEnumerable<string>.Empty;
            }

            if (this.IsEmpty)
            {
                return this.analyzer.GetTokens(text);
            }

            var enumerable = this.enumerablePool.Lease();

            enumerable.Instance.Initialize(text, enumerable);

            return enumerable.Instance;
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

            if (matchExpression is AllMatchExpression allMatchExpression)
            {
                using var tokens = SharedObjectPools.MatchTokens.Lease();
                using var expressions = SharedObjectPools.MatchExpressions.Lease();

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, new StringEnumerable(allMatchExpression.Tokens)))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions.Instance.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is string token)
                    {
                        tokens.Instance.Add(token);
                    }
                }

                if (expressions.Instance.Count == 0)
                {
                    return allMatchExpression;
                }

                try
                {
                    if (tokens.Instance.Count > 0)
                    {
#pragma warning disable CA2000 // Dispose objects before losing scope
                        expressions.Instance.Insert(0, Match.All(allMatchExpression.ScoreAggregation, tokens.Instance));
#pragma warning restore CA2000 // Dispose objects before losing scope
                    }

                    if (expressions.Instance.Count == 1)
                    {
                        return expressions.Instance[0];
                    }
                    else
                    {
                        return Match.And(allMatchExpression.ScoreAggregation, expressions.Instance);
                    }
                }
                finally
                {
                    allMatchExpression.Dispose();
                }
            }
            else if (matchExpression is AnyMatchExpression anyMatchExpression)
            {
                using var tokens = SharedObjectPools.MatchTokens.Lease();
                using var expressions = SharedObjectPools.MatchExpressions.Lease();

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, new StringEnumerable(anyMatchExpression.Tokens)))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions.Instance.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is string token)
                    {
                        tokens.Instance.Add(token);
                    }
                }

                if (expressions.Instance.Count == 0)
                {
                    return anyMatchExpression;
                }

                try
                {
                    if (tokens.Instance.Count > 0)
                    {
#pragma warning disable CA2000 // Dispose objects before losing scope
                        expressions.Instance.Insert(0, Match.Any(anyMatchExpression.ScoreAggregation, tokens.Instance));
#pragma warning restore CA2000 // Dispose objects before losing scope
                    }

                    if (expressions.Instance.Count == 1)
                    {
                        return expressions.Instance[0];
                    }
                    else
                    {
                        return Match.Or(anyMatchExpression.ScoreAggregation, expressions.Instance);
                    }
                }
                finally
                {
                    anyMatchExpression.Dispose();
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
            private readonly StringEnumerable tokens;

            public SynonymResultEnumerable(TokenNode root, StringEnumerable tokens)
            {
                if (root is null)
                {
                    throw new ArgumentNullException(nameof(root));
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
                private readonly StringEnumerable tokens;
                private StringEnumerable.Enumerator enumerator;
                private bool isEnumeratorSet;
                private bool isEnumerated;
                private TokenNode currentNode;
                private TokenNode? backtrackingNode;
                private string? previousToken;
                private SynonymResult current;

                public Enumerator(SynonymResultEnumerable source)
                {
                    this.root = source.root;
                    this.tokens = source.tokens;
                    this.enumerator = default;
                    this.isEnumeratorSet = false;
                    this.isEnumerated = false;
                    this.currentNode = this.root;
                    this.backtrackingNode = null;
                    this.previousToken = null;
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

                    if (!this.isEnumeratorSet)
                    {
                        this.enumerator = this.tokens.GetEnumerator();
                        this.isEnumeratorSet = true;
                    }

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

                    if (this.isEnumeratorSet)
                    {
                        this.enumerator.Dispose();
                        this.enumerator = default;
                    }

                    this.isEnumerated = false;
                    this.current = default;
                }

                public void Dispose()
                {
                    this.Reset();
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

        private sealed class TokenEnumerable : IDisposableEnumerable<string>
        {
            private readonly Enumerator enumerator;
            private ObjectPoolLease<TokenEnumerable> lease;
            private bool isDisposed;

            public TokenEnumerable(SynonymMap synonymMap)
            {
                this.enumerator = new Enumerator(synonymMap);
                this.lease = default;
                this.isDisposed = true;
            }

            public void Initialize(string text, ObjectPoolLease<TokenEnumerable> lease)
            {
                if (!this.isDisposed)
                {
                    throw new InvalidOperationException("Current object instance is already initialized.");
                }

                this.enumerator.Initialize(text);
                this.lease = lease;
                this.isDisposed = false;
            }

            public Enumerator GetEnumerator()
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.enumerator;
            }

            IEnumerator<string> IEnumerable<string>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            void IDisposable.Dispose()
            {
                if (!this.isDisposed)
                {
                    this.enumerator.Dispose();
                    this.lease.Dispose();
                    this.lease = default;

                    this.isDisposed = true;
                }
            }

            public sealed class Enumerator : IEnumerator<string>
            {
                private readonly IAnalyzer analyzer;
                private readonly TokenNode root;
                private string text;
                private IDisposableEnumerable<string>? tokensEnumerable;
                private SynonymResultEnumerable.Enumerator synonymResultEnumerator;
                private ListSlim<string>.Enumerator replacementTokenEnumerator;
                private string current;
                private int state;

                public Enumerator(SynonymMap synonymMap)
                {
                    this.analyzer = synonymMap.analyzer;
                    this.root = synonymMap.root;
                    this.text = default!;
                    this.synonymResultEnumerator = default;
                    this.replacementTokenEnumerator = default;
                    this.current = default!;
                    this.state = 0;
                }

                public string Current
                {
                    get
                    {
                        return this.current;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return this.current;
                    }
                }

                public void Initialize(string text)
                {
                    this.text = text;
                }

                public bool MoveNext()
                {
                    if (this.state == 0)
                    {
                        this.tokensEnumerable = this.analyzer.GetTokens(this.text);
                        this.synonymResultEnumerator = new SynonymResultEnumerable(this.root, new StringEnumerable(this.tokensEnumerable)).GetEnumerator();
                        this.state = 1;
                    }

                    if (this.state == 2)
                    {
                        if (this.replacementTokenEnumerator.MoveNext())
                        {
                            this.current = this.replacementTokenEnumerator.Current;

                            return true;
                        }

                        this.replacementTokenEnumerator.Dispose();
                        this.replacementTokenEnumerator = default;
                        this.state = 1;
                    }

                    while (this.synonymResultEnumerator.MoveNext())
                    {
                        if (this.synonymResultEnumerator.Current.Node is TokenNode node)
                        {
                            this.replacementTokenEnumerator = node.ReplacementTokens.GetEnumerator();
                            this.state = 2;

                            if (this.replacementTokenEnumerator.MoveNext())
                            {
                                this.current = this.replacementTokenEnumerator.Current;

                                return true;
                            }

                            this.replacementTokenEnumerator.Dispose();
                            this.replacementTokenEnumerator = default;
                            this.state = 1;
                        }
                        else if (this.synonymResultEnumerator.Current.Token is string token)
                        {
                            this.current = token;

                            return true;
                        }
                    }

                    this.current = default!;

                    return false;
                }

                public void Reset()
                {
                    if (this.state == 2)
                    {
                        this.replacementTokenEnumerator.Dispose();
                        this.replacementTokenEnumerator = default;
                        this.state = 1;
                    }

                    if (this.state == 1)
                    {
                        this.synonymResultEnumerator.Dispose();
                        this.synonymResultEnumerator = default;
                        this.tokensEnumerable?.Dispose();
                        this.tokensEnumerable = default;
                        this.state = 0;
                    }

                    this.current = default!;
                }

                public void Dispose()
                {
                    this.Reset();

                    this.text = default!;
                }
            }
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
                        var synonymTokens = new ListSlim<string>();
                        var isThisSynonymTokenAdded = false;

                        if (this.aggregate.MappedFrom.Count > 0 || this.aggregate.Nodes.Count > 1)
                        {
                            isThisSynonymTokenAdded = true;

                            synonymTokens.Add(this.aggregate.SynonymToken);
                        }

                        if (this.aggregate.MappedTo.Count > 0)
                        {
                            foreach (var node in this.aggregate.MappedTo)
                            {
                                if (node == this)
                                {
                                    if (isThisSynonymTokenAdded)
                                    {
                                        continue;
                                    }
                                }

                                synonymTokens.Add(node.aggregate.SynonymToken);
                            }
                        }
                        else
                        {
                            expressions.Add(Match.All(ScoreAggregation.Sum, this.tokenPath));
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
                        using var tokens = analyzer.GetTokens(phrase);

                        var tokenList = tokens.ToList();

                        if (tokenList.Count > 0)
                        {
                            if (tokenList.Count > 1 && tokenList.Count <= permutatedTokenCountThreshold)
                            {
                                foreach (var tokenPermutation in PermutationHelper.Permutate(tokenList))
                                {
                                    yield return tokenPermutation;
                                }
                            }
                            else
                            {
                                yield return tokenList;
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
