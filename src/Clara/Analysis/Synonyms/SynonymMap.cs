using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Querying;

namespace Clara.Analysis.Synonyms
{
    public class SynonymMap : ISynonymMap, ITokenFilter, IMatchExpressionFilter
    {
        private readonly SynonymNode root;

        public SynonymMap(TextField field, IEnumerable<Synonym> synonyms)
            : this(field, synonyms, new SynonymMapOptions())
        {
        }

        public SynonymMap(TextField field, IEnumerable<Synonym> synonyms, SynonymMapOptions options)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (synonyms is null)
            {
                throw new ArgumentNullException(nameof(synonyms));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.root = SynonymNode.BuildTree(field.Tokenizer, synonyms, options);
            this.Field = field;
        }

        public TextField Field { get; }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            if (this.IsEmpty())
            {
                return tokens;
            }

            return FilterEnumerable(tokens);

            IEnumerable<string> FilterEnumerable(IEnumerable<string> tokens)
            {
                foreach (var item in this.Walk(tokens))
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

            if (this.IsEmpty())
            {
                return matchExpression;
            }

            if (matchExpression is AllValuesMatchExpression allValuesMatchExpression)
            {
                var expressions = new List<MatchExpression>();
                var tokens = new HashSet<string>();

                foreach (var item in this.Walk(allValuesMatchExpression.Values))
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
                var tokens = new HashSet<string>();

                foreach (var item in this.Walk(anyValuesMatchExpression.Values))
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

        private bool IsEmpty()
        {
            return this.root.Children.Count == 0;
        }

        private IEnumerable<SynonymResult> Walk(IEnumerable<string> tokens)
        {
            var currentNode = this.root;

            foreach (var token in tokens)
            {
                if (currentNode.Children.TryGetValue(token, out var node))
                {
                    currentNode = node;
                }
                else
                {
                    if (!currentNode.IsRoot)
                    {
                        foreach (var item in Backtrack(currentNode))
                        {
                            yield return item;
                        }

                        currentNode = this.root;
                    }

                    if (currentNode.Children.TryGetValue(token, out node))
                    {
                        currentNode = node;
                    }
                    else
                    {
                        yield return new SynonymResult(token);
                    }
                }
            }

            if (!currentNode.IsRoot)
            {
                foreach (var item in Backtrack(currentNode))
                {
                    yield return item;
                }
            }

            static IEnumerable<SynonymResult> Backtrack(SynonymNode node)
            {
                if (node.HasSynonyms)
                {
                    yield return new SynonymResult(node);
                }
                else
                {
                    if (!node.Parent.IsRoot)
                    {
                        foreach (var item in Backtrack(node.Parent))
                        {
                            yield return item;
                        }
                    }

                    yield return new SynonymResult(node.Token);
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

            public static SynonymNode BuildTree(ITokenizer tokenizer, IEnumerable<Synonym> synonyms, SynonymMapOptions options)
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

                        foreach (var token in tokenizer.GetTokens(phrase))
                        {
                            tokenizedPhrase.Add(token);
                        }

                        if (tokenizedPhrase.Count >= 1)
                        {
                            tokenizedPhrases.Add(tokenizedPhrase);
                        }
                    }

                    if (synonym is EquivalencySynonym)
                    {
                        if (tokenizedPhrases.Count >= 2)
                        {
                            foreach (var tokenizedPhrase in tokenizedPhrases)
                            {
                                foreach (var tokenPermutation in options.GetPhraseTokensPermutations(tokenizedPhrase))
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
                        if (tokenizedPhrases.Count >= 1)
                        {
                            var replacementTokens = new List<string>();

                            foreach (var token in tokenizer.GetTokens(explicitMappingSynonym.MappedPhrase))
                            {
                                replacementTokens.Add(token);
                            }

                            if (replacementTokens.Count < 1)
                            {
                                continue;
                            }

                            tokenizedPhrases.Add(replacementTokens);

                            foreach (var tokenizedPhrase in tokenizedPhrases)
                            {
                                foreach (var tokenPermutation in options.GetPhraseTokensPermutations(tokenizedPhrase))
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
                }

                return root;
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
