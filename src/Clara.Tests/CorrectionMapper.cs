using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Tests
{
    public sealed class CorrectionDocument
    {
        public CorrectionDocument(string key, string text, string brand, params PhraseWeight[] phrases)
        {
            this.Key = key;
            this.Text = text;
            this.Brand = brand;
            this.Phrases = phrases;
        }

        public string Key { get; }

        public string Text { get; }

        public string Brand { get; }

        public IReadOnlyList<PhraseWeight> Phrases { get; }
    }

    public sealed class CorrectionMapper : IIndexMapper<CorrectionDocument>
    {
        public CorrectionMapper(
            IEnumerable<KeyValuePair<string, string>>? variants = null,
            IEnumerable<KeyValuePair<string, string>>? phraseVariants = null,
            IEnumerable<KeyValuePair<string, string>>? phraseAliases = null,
            bool validatePhrases = true)
        {
            this.Text =
                new TextField<CorrectionDocument>(
                    x => x.Text,
                    new PorterAnalyzer(),
                    similarity: Similarity.TF,
                    indexScoreAggregation: ScoreAggregation.Max,
                    searchScoreAggregation: ScoreAggregation.Sum,
                    tokenCorrection: new TokenCorrection(SurfaceAnalyzer, variants: variants));

            this.Phrases =
                new PhraseField<CorrectionDocument>(
                    x => x.Phrases,
                    SurfaceAnalyzer,
                    validationField: validatePhrases ? this.Text : null,
                    variants: phraseVariants,
                    aliases: phraseAliases);
        }

        public static IAnalyzer SurfaceAnalyzer { get; } = new Analyzer(new StandardTokenizer(), new LowerInvariantTokenFilter());

        public static IReadOnlyList<CorrectionDocument> Documents { get; } =
            new[]
            {
                new CorrectionDocument("chocolate-bar", "chocolate bar", "alpha", new PhraseWeight("chocolate", 30), new PhraseWeight("chocolate bar", 5), new PhraseWeight("unicorn", 100)),
                new CorrectionDocument("chocolate-cookies", "chocolate cookies", "beta", new PhraseWeight("chocolate", 0), new PhraseWeight("chocolate cookies", 30)),
                new CorrectionDocument("cherry-juice", "cherry juice", "alpha", new PhraseWeight("cherry", 40), new PhraseWeight("cherry juice", 10)),
                new CorrectionDocument("red-apple", "red apple", "gamma", new PhraseWeight("red apple", 20)),
                new CorrectionDocument("redwood-table", "redwood table", "gamma", new PhraseWeight("redwood table", 5)),
                new CorrectionDocument("slow-cookers", "slow cookers", "gamma", new PhraseWeight("slow cookers", 0)),
                new CorrectionDocument("color-pencils", "color pencils", "gamma"),
                new CorrectionDocument("cable-ties", "cable ties", "gamma"),
            };

        public TextField<CorrectionDocument> Text { get; }

        public PhraseField<CorrectionDocument> Phrases { get; }

        public KeywordField<CorrectionDocument> Brand { get; } = new(x => x.Brand, isFilterable: true);

        public IEnumerable<Field> GetFields()
        {
            yield return this.Text;
            yield return this.Phrases;
            yield return this.Brand;
        }

        public string GetDocumentKey(CorrectionDocument item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.Key;
        }

        public CorrectionDocument GetDocument(CorrectionDocument item)
        {
            return item;
        }
    }
}
