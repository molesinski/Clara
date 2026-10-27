using Clara.Analysis;

namespace Clara.Mapping
{
    public sealed class TokenCorrection
    {
        public const int DefaultMinimumVariantLength = 5;

        public TokenCorrection(
            IAnalyzer analyzer,
            TokenMatching? matching = null,
            IEnumerable<KeyValuePair<string, string>>? variants = null,
            int minimumVariantLength = DefaultMinimumVariantLength)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (minimumVariantLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumVariantLength));
            }

            this.Analyzer = analyzer;
            this.Matching = matching ?? TokenMatching.Default;
            this.Variants = variants?.ToList() ?? new List<KeyValuePair<string, string>>();
            this.MinimumVariantLength = minimumVariantLength;
        }

        public IAnalyzer Analyzer { get; }

        public TokenMatching Matching { get; }

        public IReadOnlyCollection<KeyValuePair<string, string>> Variants { get; }

        public int MinimumVariantLength { get; }
    }
}
