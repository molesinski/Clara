using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Tests
{
    public sealed class CorrectionDocument
    {
        public CorrectionDocument(string key, string text, string brand)
        {
            this.Key = key;
            this.Text = text;
            this.Brand = brand;
        }

        public string Key { get; }

        public string Text { get; }

        public string Brand { get; }
    }

    public sealed class CorrectionMapper : IIndexMapper<CorrectionDocument>
    {
        public CorrectionMapper(IEnumerable<KeyValuePair<string, string>>? variants = null)
        {
            this.Text =
                new TextField<CorrectionDocument>(
                    x => x.Text,
                    new PorterAnalyzer(),
                    similarity: Similarity.TF,
                    indexScoreAggregation: ScoreAggregation.Max,
                    searchScoreAggregation: ScoreAggregation.Sum,
                    tokenCorrection: new TokenCorrection(SurfaceAnalyzer, variants: variants));
        }

        public static IAnalyzer SurfaceAnalyzer { get; } = new Analyzer(new StandardTokenizer(), new LowerInvariantTokenFilter());

        public static IReadOnlyList<CorrectionDocument> Documents { get; } =
            new[]
            {
                new CorrectionDocument("chocolate-bar", "chocolate bar", "alpha"),
                new CorrectionDocument("chocolate-cookies", "chocolate cookies", "beta"),
                new CorrectionDocument("cherry-juice", "cherry juice", "alpha"),
                new CorrectionDocument("red-apple", "red apple", "gamma"),
                new CorrectionDocument("redwood-table", "redwood table", "gamma"),
                new CorrectionDocument("slow-cookers", "slow cookers", "gamma"),
                new CorrectionDocument("color-pencils", "color pencils", "gamma"),
                new CorrectionDocument("cable-ties", "cable ties", "gamma"),
            };

        public TextField<CorrectionDocument> Text { get; }

        public KeywordField<CorrectionDocument> Brand { get; } = new(x => x.Brand, isFilterable: true);

        public IEnumerable<Field> GetFields()
        {
            yield return this.Text;
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
