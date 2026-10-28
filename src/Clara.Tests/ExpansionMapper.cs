using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;

namespace Clara.Tests
{
    public sealed class ExpansionDocument
    {
        public ExpansionDocument(string key, string text, string brand, params PhraseWeight[] phrases)
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

    public sealed class ExpansionMapper : IIndexMapper<ExpansionDocument>
    {
        public ExpansionMapper(
            IEnumerable<KeyValuePair<string, string>>? variants = null,
            IEnumerable<KeyValuePair<string, string>>? phraseVariants = null,
            IEnumerable<KeyValuePair<string, string>>? phraseAliases = null,
            bool validatePhrases = true,
            IEnumerable<Synonym>? synonyms = null)
        {
            var analyzer = new PorterAnalyzer();

            this.Text =
                new TextField<ExpansionDocument>(
                    x => x.Text,
                    analyzer,
                    synonymMap: synonyms is null ? null : new SynonymMap(analyzer, synonyms),
                    similarity: Similarity.TF,
                    indexScoreAggregation: ScoreAggregation.Max,
                    searchScoreAggregation: ScoreAggregation.Sum,
                    tokenExpansion: new TokenExpansion(SurfaceAnalyzer, variants: variants));

            this.Phrases =
                new PhraseField<ExpansionDocument>(
                    x => x.Phrases,
                    SurfaceAnalyzer,
                    validationField: validatePhrases ? this.Text : null,
                    variants: phraseVariants,
                    aliases: phraseAliases);
        }

        public static IAnalyzer SurfaceAnalyzer { get; } = new Analyzer(new StandardTokenizer(), new LowerInvariantTokenFilter());

        public static IReadOnlyList<ExpansionDocument> Documents { get; } =
            new[]
            {
                new ExpansionDocument("chocolate-bar", "chocolate bar", "alpha", new PhraseWeight("chocolate", 30), new PhraseWeight("chocolate bar", 5), new PhraseWeight("unicorn", 100)),
                new ExpansionDocument("chocolate-cookies", "chocolate cookies", "beta", new PhraseWeight("chocolate", 0), new PhraseWeight("chocolate cookies", 30)),
                new ExpansionDocument("cherry-juice", "cherry juice", "alpha", new PhraseWeight("cherry", 40), new PhraseWeight("cherry juice", 10)),
                new ExpansionDocument("red-apple", "red apple", "gamma", new PhraseWeight("red apple", 20)),
                new ExpansionDocument("redwood-table", "redwood table", "gamma", new PhraseWeight("redwood table", 5)),
                new ExpansionDocument("slow-cookers", "slow cookers", "gamma", new PhraseWeight("slow cookers", 0)),
                new ExpansionDocument("color-pencils", "color pencils", "gamma"),
                new ExpansionDocument("cable-ties", "cable ties", "gamma"),
            };

        public TextField<ExpansionDocument> Text { get; }

        public PhraseField<ExpansionDocument> Phrases { get; }

        public KeywordField<ExpansionDocument> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);

        public IEnumerable<Field> GetFields()
        {
            yield return this.Text;
            yield return this.Phrases;
            yield return this.Brand;
        }

        public string GetDocumentKey(ExpansionDocument item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.Key;
        }

        public ExpansionDocument GetDocument(ExpansionDocument item)
        {
            return item;
        }
    }
}
