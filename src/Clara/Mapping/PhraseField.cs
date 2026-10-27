using Clara.Analysis;
using Clara.Storage;

namespace Clara.Mapping
{
    public abstract class PhraseField : Field
    {
        public const string DefaultHighlightStart = "<b>";
        public const string DefaultHighlightEnd = "</b>";

        internal PhraseField(
            IAnalyzer analyzer,
            TextField? validationField,
            TokenMatching? matching,
            IEnumerable<string>? stopwords,
            IEnumerable<KeyValuePair<string, string>>? variants,
            IEnumerable<KeyValuePair<string, string>>? aliases,
            int minimumVariantLength,
            string highlightStart,
            string highlightEnd)
        {
            if (analyzer is null)
            {
                throw new ArgumentNullException(nameof(analyzer));
            }

            if (minimumVariantLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumVariantLength));
            }

            if (highlightStart is null)
            {
                throw new ArgumentNullException(nameof(highlightStart));
            }

            if (highlightEnd is null)
            {
                throw new ArgumentNullException(nameof(highlightEnd));
            }

            this.Analyzer = analyzer;
            this.ValidationField = validationField;
            this.Matching = matching ?? TokenMatching.Default;
            this.Stopwords = stopwords?.ToList() ?? new List<string>();
            this.Variants = variants?.ToList() ?? new List<KeyValuePair<string, string>>();
            this.Aliases = aliases?.ToList() ?? new List<KeyValuePair<string, string>>();
            this.MinimumVariantLength = minimumVariantLength;
            this.HighlightStart = highlightStart;
            this.HighlightEnd = highlightEnd;
        }

        public IAnalyzer Analyzer { get; }

        public TextField? ValidationField { get; }

        public TokenMatching Matching { get; }

        public IReadOnlyCollection<string> Stopwords { get; }

        public IReadOnlyCollection<KeyValuePair<string, string>> Variants { get; }

        public IReadOnlyCollection<KeyValuePair<string, string>> Aliases { get; }

        public int MinimumVariantLength { get; }

        public string HighlightStart { get; }

        public string HighlightEnd { get; }
    }

    public sealed class PhraseField<TSource> : PhraseField
    {
        public PhraseField(
            Func<TSource, IEnumerable<PhraseWeight>?> valueMapper,
            IAnalyzer analyzer,
            TextField? validationField = null,
            TokenMatching? matching = null,
            IEnumerable<string>? stopwords = null,
            IEnumerable<KeyValuePair<string, string>>? variants = null,
            IEnumerable<KeyValuePair<string, string>>? aliases = null,
            int minimumVariantLength = TokenCorrection.DefaultMinimumVariantLength,
            string highlightStart = DefaultHighlightStart,
            string highlightEnd = DefaultHighlightEnd)
            : base(analyzer, validationField, matching, stopwords, variants, aliases, minimumVariantLength, highlightStart, highlightEnd)
        {
            if (valueMapper is null)
            {
                throw new ArgumentNullException(nameof(valueMapper));
            }

            this.ValueMapper = valueMapper;
        }

        internal Func<TSource, IEnumerable<PhraseWeight>?> ValueMapper { get; }

        internal override FieldStoreBuilder CreateFieldStoreBuilder(TokenEncoderBuilder tokenEncoderBuilder)
        {
            return new PhraseFieldStoreBuilder<TSource>(this);
        }
    }
}
