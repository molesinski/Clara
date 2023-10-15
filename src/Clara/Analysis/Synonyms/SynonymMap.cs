using Clara.Querying;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap : ISynonymMap
    {
        public const int MaximumPermutatedTokenCount = 5;

        private readonly HashSet<Synonym> synonyms = new();
        private readonly StringPoolSlim stringPool = new();
        private readonly IEnumerable<AnalyzerTerm> empty;
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

            if (permutatedTokenCountThreshold < 1 || permutatedTokenCountThreshold > MaximumPermutatedTokenCount)
            {
                throw new ArgumentOutOfRangeException(nameof(permutatedTokenCountThreshold));
            }

            foreach (var synonym in synonyms)
            {
                if (synonym is null)
                {
                    throw new ArgumentException("Synonym values must not be null.", nameof(synonyms));
                }

                this.synonyms.Add(synonym);
            }

            this.empty = new AnalyzerTermEnumerable(this, string.Empty);
            this.analyzer = analyzer;
            this.root = TokenNode.Build(analyzer, this.synonyms, permutatedTokenCountThreshold, this.stringPool);
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

        public IEnumerable<AnalyzerTerm> GetTerms(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return this.empty;
            }

            return new AnalyzerTermEnumerable(this, text);
        }

        public void Process(SearchMode mode, IList<SearchTerm> terms)
        {
            if (terms is null)
            {
                throw new ArgumentNullException(nameof(terms));
            }

            if (terms.Count == 0 || this.root.Children.Count == 0)
            {
                return;
            }

            using var tempTerms = SharedObjectPools.SearchTerms.Lease();
            var hasMatchExpressions = false;

            foreach (var item in new SynonymSearchTermEnumerable(this, new PrimitiveEnumerable<SearchTerm>(terms)))
            {
                if (item.Node is TokenNode node)
                {
                    tempTerms.Instance.Add(new SearchTerm(item.Position, node.MatchExpression));
                    hasMatchExpressions = true;
                }
                else if (item.Token is string token)
                {
                    tempTerms.Instance.Add(new SearchTerm(item.Position, token));
                }
            }

            if (hasMatchExpressions)
            {
                terms.Clear();

                foreach (var token in tempTerms.Instance)
                {
                    terms.Add(token);
                }
            }
        }

        public string? ToReadOnly(Token token)
        {
            if (this.stringPool.TryGet(token, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
