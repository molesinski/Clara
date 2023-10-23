using Clara.Querying;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed partial class SynonymMap : ISynonymMap
    {
        public const int MaximumPermutatedTokenCount = 5;

        private readonly HashSet<Synonym> synonyms = new();
        private readonly StringPoolSlim stringPool = new();
        private readonly IAnalyzer analyzer;
        private readonly TokenNode root;
        private readonly ObjectPool<SearchTermEnumerable> searchTermEnumerablePool;

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

            this.analyzer = analyzer;
            this.root = TokenNode.Build(analyzer, this.synonyms, permutatedTokenCountThreshold, this.stringPool);
            this.searchTermEnumerablePool = new(() => new(this));
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

        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource(this);
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
            using var searchTermEnumerable = this.searchTermEnumerablePool.Lease();

            tempTerms.Instance.AddRange(searchTermEnumerable.Instance.GetTerms(terms));
            tempTerms.Instance.Sort(SearchTermComparer.Instance);

            terms.Clear();

            foreach (var token in tempTerms.Instance)
            {
                terms.Add(token);
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
