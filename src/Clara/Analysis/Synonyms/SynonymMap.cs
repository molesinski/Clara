using System;
using System.Collections;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public class SynonymMap : ISynonymMap, IAnalyzer, IMatchExpressionFilter
    {
        private readonly TextField field;
        private readonly IAnalyzer analyzer;
        private readonly SynonymNode root;

        public SynonymMap(TextField field, IEnumerable<Synonym> synonyms, int maximumPermutatedPhraseTokenCount = 1)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (synonyms is null)
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            if (maximumPermutatedPhraseTokenCount < 1 || maximumPermutatedPhraseTokenCount > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumPermutatedPhraseTokenCount));
            }

            this.field = field;
            this.analyzer = field.Analyzer;
            this.root = SynonymNode.BuildTree(this.analyzer, synonyms, maximumPermutatedPhraseTokenCount);
        }

        public TextField Field
        {
            get
            {
                return this.field;
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
                    if (item.Node is SynonymNode node)
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

        public MatchExpression Filter(MatchExpression matchExpression)
        {
            if (matchExpression is null)
            {
                throw new ArgumentNullException(nameof(matchExpression));
            }

            if (this.IsEmpty)
            {
                return matchExpression;
            }

            if (matchExpression is AllValuesMatchExpression allValuesMatchExpression)
            {
                var expressions = new List<MatchExpression>();
                var tokens = new List<string>();

                foreach (var item in new SynonymResultEnumerable(this.root, allValuesMatchExpression.Values))
                {
                    if (item.Node is SynonymNode node)
                    {
                        expressions.Add(node.MatchExpression);
                    }
                    else if (item.Token is string token)
                    {
                        tokens.Add(token);
                    }
                }

                if (tokens.Count > 0)
                {
                    expressions.Insert(0, Match.All(tokens));
                }

                return Match.And(expressions);
            }
            else if (matchExpression is AnyValuesMatchExpression anyValuesMatchExpression)
            {
                var expressions = new List<MatchExpression>();
                var tokens = new List<string>();

                foreach (var item in new SynonymResultEnumerable(this.root, anyValuesMatchExpression.Values))
                {
                    if (item.Node is SynonymNode node)
                    {
                        expressions.Add(node.MatchExpression);
                    }
                    else if (item.Token is string token)
                    {
                        tokens.Add(token);
                    }
                }

                if (tokens.Count > 0)
                {
                    expressions.Insert(0, Match.Any(tokens));
                }

                return Match.Or(expressions);
            }
            else
            {
                return matchExpression;
            }
        }

        private readonly struct SynonymResultEnumerable : IEnumerable<SynonymResult>
        {
            private readonly SynonymNode root;
            private readonly IEnumerable<string> tokens;

            public SynonymResultEnumerable(SynonymNode root, IEnumerable<string> tokens)
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

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator<SynonymResult> IEnumerable<SynonymResult>.GetEnumerator()
            {
                return new Enumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this);
            }

            public struct Enumerator : IEnumerator<SynonymResult>
            {
                private readonly SynonymNode root;
                private readonly IEnumerable<string> tokens;
                private SynonymNode currentNode;
                private SynonymNode? backtrackingNode;
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

                public SynonymResult Current
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

            public SynonymResult(SynonymNode node)
            {
                if (node is null)
                {
                    throw new ArgumentNullException(nameof(node));
                }

                this.Token = null;
                this.Node = node;
            }

            public string? Token { get; }

            public SynonymNode? Node { get; }
        }

        private class SynonymNode
        {
            private static readonly string SynonymTokenPrefix = string.Concat("__", Guid.NewGuid().ToString().Substring(0, 8), "__");

            private readonly string? token;
            private readonly SynonymNode? parent;
            private readonly string[] pathTokens;
            private MatchExpression? matchExpression;
            private IEnumerable<string>? replacementTokens;

            private SynonymNode()
            {
                this.token = null;
                this.parent = null;
                this.pathTokens = Array.Empty<string>();
            }

            private SynonymNode(string token, SynonymNode parent)
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
                        throw new InvalidOperationException("Unable to retrieve token value for root synonym node.");
                    }

                    return this.token;
                }
            }

            public SynonymNode Parent
            {
                get
                {
                    if (this.parent is null)
                    {
                        throw new InvalidOperationException("Unable to retrieve parent node value for root synonym node.");
                    }

                    return this.parent;
                }
            }

            public Dictionary<string, SynonymNode> Children { get; } = new();

            public Dictionary<Synonym, SynonymTokens> SynonymTokens { get; } = new();

            public bool HasSynonyms
            {
                get
                {
                    return this.SynonymTokens.Count > 0;
                }
            }

            public MatchExpression MatchExpression
            {
                get
                {
                    if (this.matchExpression is null)
                    {
                        var expressions = new List<MatchExpression>();
                        var addedSynonymTokens = new HashSet<string>();
                        var isPathTokensAdded = false;

                        foreach (var pair in this.SynonymTokens)
                        {
                            var synonym = pair.Key;
                            var synonymTokens = pair.Value;

                            if (synonym is EquivalencySynonym)
                            {
                                if (!isPathTokensAdded)
                                {
                                    expressions.Add(Match.All(this.pathTokens));
                                    isPathTokensAdded = true;
                                }

                                if (!addedSynonymTokens.Contains(synonymTokens.SyntheticToken))
                                {
                                    expressions.Add(Match.All(synonymTokens.SyntheticToken));
                                    addedSynonymTokens.Add(synonymTokens.SyntheticToken);
                                }
                            }
                            else if (synonym is ExplicitMappingSynonym)
                            {
                                if (!addedSynonymTokens.Contains(synonymTokens.SyntheticToken))
                                {
                                    expressions.Add(Match.All(synonymTokens.SyntheticToken));
                                    addedSynonymTokens.Add(synonymTokens.SyntheticToken);
                                }
                            }
                        }

                        this.matchExpression = Match.Or(expressions);
                    }

                    return this.matchExpression;
                }
            }

            public IEnumerable<string> ReplacementTokens
            {
                get
                {
                    if (this.replacementTokens is null)
                    {
                        var replacementTokens = new HashSet<string>();

                        foreach (var pair in this.SynonymTokens)
                        {
                            var synonym = pair.Key;
                            var synonymTokens = pair.Value;

                            if (synonym is EquivalencySynonym)
                            {
                                replacementTokens.UnionWith(this.pathTokens);
                                replacementTokens.Add(synonymTokens.SyntheticToken);
                            }
                            else if (synonym is ExplicitMappingSynonym)
                            {
                                replacementTokens.Add(synonymTokens.SyntheticToken);
                                replacementTokens.UnionWith(synonymTokens.ReplacementTokens);
                            }
                        }

                        this.replacementTokens = replacementTokens;
                    }

                    return this.replacementTokens;
                }
            }

            public static SynonymNode BuildTree(IAnalyzer analyzer, IEnumerable<Synonym> synonyms, int maximumPermutatedPhraseTokenCount)
            {
                var nextSynonymId = 1;
                var root = new SynonymNode();

                foreach (var synonym in synonyms)
                {
                    if (synonym is null)
                    {
                        continue;
                    }

                    var syntheticToken = string.Concat(SynonymTokenPrefix, nextSynonymId++);
                    var tokenizedPhrases = new List<List<string>>();

                    foreach (var phrase in synonym.Phrases)
                    {
                        var tokenizedPhrase = new List<string>();

                        foreach (var token in analyzer.GetTokens(phrase))
                        {
                            tokenizedPhrase.Add(token);
                        }

                        if (tokenizedPhrase.Count > 0)
                        {
                            tokenizedPhrases.Add(tokenizedPhrase);
                        }
                    }

                    if (synonym is EquivalencySynonym)
                    {
                        if (tokenizedPhrases.Count > 1)
                        {
                            foreach (var tokenizedPhrase in tokenizedPhrases)
                            {
                                foreach (var tokenPermutation in GetPhraseTokensPermutations(tokenizedPhrase))
                                {
                                    var node = root;

                                    foreach (var token in tokenPermutation)
                                    {
                                        if (!node.Children.TryGetValue(token, out var child))
                                        {
                                            node.Children.Add(token, child = new SynonymNode(token, node));
                                        }

                                        node = child;
                                    }

                                    if (!node.SynonymTokens.ContainsKey(synonym))
                                    {
                                        node.SynonymTokens.Add(synonym, new SynonymTokens(syntheticToken));
                                    }
                                }
                            }
                        }
                    }
                    else if (synonym is ExplicitMappingSynonym explicitMappingSynonym)
                    {
                        if (tokenizedPhrases.Count > 0)
                        {
                            var replacementTokens = new List<string>();

                            foreach (var token in analyzer.GetTokens(explicitMappingSynonym.MappedPhrase))
                            {
                                replacementTokens.Add(token);
                            }

                            if (!(replacementTokens.Count > 0))
                            {
                                continue;
                            }

                            tokenizedPhrases.Add(replacementTokens);

                            foreach (var tokenizedPhrase in tokenizedPhrases)
                            {
                                foreach (var tokenPermutation in GetPhraseTokensPermutations(tokenizedPhrase))
                                {
                                    var node = root;

                                    foreach (var token in tokenPermutation)
                                    {
                                        if (!node.Children.TryGetValue(token, out var child))
                                        {
                                            node.Children.Add(token, child = new SynonymNode(token, node));
                                        }

                                        node = child;
                                    }

                                    if (!node.SynonymTokens.ContainsKey(synonym))
                                    {
                                        node.SynonymTokens.Add(synonym, new SynonymTokens(syntheticToken, replacementTokens));
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

                IEnumerable<IEnumerable<string>> GetPhraseTokensPermutations(List<string> phraseTokens)
                {
                    return
                        maximumPermutatedPhraseTokenCount > 1 && phraseTokens.Count <= maximumPermutatedPhraseTokenCount
                            ? PermutationHelper.Permutate(phraseTokens)
                            : PermutationHelper.Identity(phraseTokens);
                }
            }
        }

        private class SynonymTokens
        {
            public SynonymTokens(string syntheticToken)
            {
                this.SyntheticToken = syntheticToken;
                this.ReplacementTokens = Array.Empty<string>();
            }

            public SynonymTokens(string syntheticToken, List<string> replacementTokens)
            {
                this.SyntheticToken = syntheticToken;
                this.ReplacementTokens = replacementTokens;
            }

            public string SyntheticToken { get; }

            public IEnumerable<string> ReplacementTokens { get; }
        }
    }
}
