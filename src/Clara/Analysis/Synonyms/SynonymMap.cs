using Clara.Analysis.MatchExpressions;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap : ISynonymMap
    {
        public const int MaximumPermutatedTokenCount = 5;

        private readonly HashSet<Synonym> synonyms;
        private readonly IEnumerable<Token> emptyEnumerable;
        private readonly IAnalyzer analyzer;
        private readonly TokenNode root;
        private readonly DictionarySlim<Token, string> tokenMap;

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

            if (permutatedTokenCountThreshold < 1 || permutatedTokenCountThreshold > MaximumPermutatedTokenCount)
            {
                throw new ArgumentOutOfRangeException(nameof(permutatedTokenCountThreshold));
            }

            this.synonyms = new();

            foreach (var synonym in synonyms)
            {
                if (synonym is null)
                {
                    throw new ArgumentException("Synonym values must not be null.", nameof(synonyms));
                }

                this.synonyms.Add(synonym);
            }

            this.emptyEnumerable = new TokenEnumerable(this, string.Empty);
            this.analyzer = analyzer;
            this.root = TokenNode.Build(analyzer, this.synonyms, permutatedTokenCountThreshold, out this.tokenMap);
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

        public TokenEnumerable GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new TokenEnumerable(this, text);
        }

        IEnumerable<Token> IAnalyzer.GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return this.emptyEnumerable;
            }

            return new TokenEnumerable(this, text);
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

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, new PrimitiveEnumerable<Token>(allMatchExpression.Tokens)))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions.Instance.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is Token token)
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

                foreach (var synonymResult in new SynonymResultEnumerable(this.root, new PrimitiveEnumerable<Token>(anyMatchExpression.Tokens)))
                {
                    if (synonymResult.Node is TokenNode node)
                    {
                        expressions.Instance.Add(node.MatchExpression);
                    }
                    else if (synonymResult.Token is Token token)
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

        public Token? ToReadOnly(Token token)
        {
            if (this.tokenMap.TryGetValue(token, out var value))
            {
                return new Token(value);
            }

            return null;
        }
    }
}
