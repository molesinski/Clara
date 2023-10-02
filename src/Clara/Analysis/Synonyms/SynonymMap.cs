﻿using System.Collections;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public class SynonymMap : ISynonymMap
    {
        private readonly IAnalyzer analyzer;
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

            if (permutatedTokenCountThreshold < 1 || permutatedTokenCountThreshold > PermutationHelper.MaximumPermutatedTokenCount)
            {
                throw new ArgumentOutOfRangeException(nameof(permutatedTokenCountThreshold));
            }

            this.analyzer = analyzer;
            this.root = TokenNode.BuildTree(analyzer, synonyms, permutatedTokenCountThreshold);
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
            private const string SynonymTokenPrefix = "__SYNONYM__";

            private readonly string? token;
            private readonly TokenNode? parent;
            private readonly string[] pathTokens;
            private MatchExpression? matchExpression;
            private ListSlim<string>? replacementTokens;

            private TokenNode()
            {
                this.token = null;
                this.parent = null;
                this.pathTokens = Array.Empty<string>();
            }

            private TokenNode(string token, TokenNode parent)
            {
                if (parent is null)
                {
                    throw new ArgumentNullException(nameof(parent));
                }

                this.token = token;
                this.parent = parent;

                var length = parent.pathTokens.Length;
                var pathTokens = new string[length + 1];
                Array.Copy(parent.pathTokens, pathTokens, length);
                pathTokens[length] = token;

                this.pathTokens = pathTokens;
            }

            public bool IsRoot
            {
                get
                {
                    return this.parent is null;
                }
            }

            public string Token
            {
                get
                {
                    if (this.token is null)
                    {
                        throw new InvalidOperationException("Unable to retrieve token value for tree root node.");
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
                        throw new InvalidOperationException("Unable to retrieve parent node value for tree root node.");
                    }

                    return this.parent;
                }
            }

            public Dictionary<string, TokenNode> Children { get; } = new();

            public Dictionary<Synonym, SynonymTokens> Synonyms { get; } = new();

            public bool HasSynonyms
            {
                get
                {
                    return this.Synonyms.Count > 0;
                }
            }

            public MatchExpression MatchExpression
            {
                get
                {
                    if (this.matchExpression is null)
                    {
                        var expressions = new ListSlim<MatchExpression>();
                        var addedSynonymTokens = new HashSetSlim<string>();
                        var isPathTokensAdded = false;

                        foreach (var pair in this.Synonyms)
                        {
                            var synonym = pair.Key;
                            var synonymTokens = pair.Value;

                            if (!addedSynonymTokens.Contains(synonymTokens.SynonymToken))
                            {
                                expressions.Add(Match.All(synonymTokens.SynonymToken));
                                addedSynonymTokens.Add(synonymTokens.SynonymToken);
                            }

                            if (synonym is EquivalencySynonym)
                            {
                                if (!isPathTokensAdded)
                                {
                                    expressions.Add(Match.All(this.pathTokens));
                                    isPathTokensAdded = true;
                                }
                            }
                            else if (synonym is ExplicitMappingSynonym)
                            {
                            }
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
                        var isPathTokensAdded = false;

                        foreach (var pair in this.Synonyms)
                        {
                            var synonym = pair.Key;
                            var synonymTokens = pair.Value;

                            replacementTokens.Add(synonymTokens.SynonymToken);

                            if (synonym is EquivalencySynonym)
                            {
                                if (!isPathTokensAdded)
                                {
                                    replacementTokens.AddRange(this.pathTokens);
                                    isPathTokensAdded = true;
                                }
                            }
                            else if (synonym is ExplicitMappingSynonym)
                            {
                                replacementTokens.AddRange(synonymTokens.ReplacementTokens);
                            }
                        }

                        this.replacementTokens = replacementTokens;
                    }

                    return this.replacementTokens;
                }
            }

            public static TokenNode BuildTree(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int permutatedTokenCountThreshold)
            {
                var nextSynonymTokenId = 1;
                var root = new TokenNode();

                foreach (var synonym in synonyms)
                {
                    if (synonym is null)
                    {
                        continue;
                    }

                    var synonymToken = string.Concat(SynonymTokenPrefix, nextSynonymTokenId++);
                    var phrases = new ListSlim<ListSlim<string>>();

                    foreach (var phrase in synonym.Phrases)
                    {
                        var tokens = new ListSlim<string>();

                        foreach (var token in analyzer.GetTokens(phrase))
                        {
                            tokens.Add(token);
                        }

                        if (tokens.Count > 0)
                        {
                            phrases.Add(tokens);
                        }
                    }

                    if (synonym is EquivalencySynonym)
                    {
                        if (phrases.Count > 1)
                        {
                            foreach (var phrase in phrases)
                            {
                                foreach (var phrasePermutation in GetPhraseTokensPermutations(phrase))
                                {
                                    var node = root;

                                    foreach (var token in phrasePermutation)
                                    {
                                        if (!node.Children.TryGetValue(token, out var child))
                                        {
                                            node.Children.Add(token, child = new TokenNode(token, node));
                                        }

                                        node = child;
                                    }

                                    if (!node.Synonyms.ContainsKey(synonym))
                                    {
                                        node.Synonyms.Add(synonym, new SynonymTokens(synonymToken));
                                    }
                                }
                            }
                        }
                    }
                    else if (synonym is ExplicitMappingSynonym explicitMappingSynonym)
                    {
                        if (phrases.Count > 0)
                        {
                            var replacementTokens = new ListSlim<string>();

                            foreach (var mappedPhrase in explicitMappingSynonym.MappedPhrases)
                            {
                                foreach (var token in analyzer.GetTokens(mappedPhrase))
                                {
                                    replacementTokens.Add(token);
                                }
                            }

                            if (!(replacementTokens.Count > 0))
                            {
                                continue;
                            }

                            phrases.Add(replacementTokens);

                            foreach (var tokenizedPhrase in phrases)
                            {
                                foreach (var tokenPermutation in GetPhraseTokensPermutations(tokenizedPhrase))
                                {
                                    var node = root;

                                    foreach (var token in tokenPermutation)
                                    {
                                        if (!node.Children.TryGetValue(token, out var child))
                                        {
                                            node.Children.Add(token, child = new TokenNode(token, node));
                                        }

                                        node = child;
                                    }

                                    if (!node.Synonyms.ContainsKey(synonym))
                                    {
                                        node.Synonyms.Add(synonym, new SynonymTokens(synonymToken, replacementTokens));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unsupported synonym type definition encountered '{synonym.GetType().FullName}'.");
                    }
                }

                return root;

                IEnumerable<string[]> GetPhraseTokensPermutations(ListSlim<string> tokens)
                {
                    if (permutatedTokenCountThreshold > 1 && tokens.Count <= permutatedTokenCountThreshold)
                    {
                        foreach (var tokenPermutation in PermutationHelper.Permutate(tokens))
                        {
                            yield return tokenPermutation;
                        }
                    }
                    else
                    {
                        yield return tokens.ToArray();
                    }
                }
            }
        }

        private sealed class SynonymTokens
        {
            private static readonly ListSlim<string> EmptyReplacementTokens = new();

            public SynonymTokens(string synonymToken)
            {
                this.SynonymToken = synonymToken;
                this.ReplacementTokens = EmptyReplacementTokens;
            }

            public SynonymTokens(string synonymToken, ListSlim<string> replacementTokens)
            {
                this.SynonymToken = synonymToken;
                this.ReplacementTokens = replacementTokens;
            }

            public string SynonymToken { get; }

            public ListSlim<string> ReplacementTokens { get; }
        }
    }
}
