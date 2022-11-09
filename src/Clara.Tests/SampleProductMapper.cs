using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Tests
{
    public class SampleProductMapper : IIndexMapper<SampleProduct, SampleProduct>
    {
        public static readonly IAnalyzer Analyzer =
            new Analyzer(
                new BasicTokenizer(),
                new LowerInvariantTokenFilter(),
                new CachingTokenFilter(),
                new EnglishPossesiveTokenFilter(),
                new EnglishStopTokenFilter(),
                new RequireLengthTokenFilter(),
                new RequireNonDigitsTokenFilter(),
                new EnglishPorterStemTokenFilter());

        public static readonly TextField<SampleProduct> Text = new(o => o.GetText(), Analyzer);
        public static readonly DoubleField<SampleProduct> Price = new(o => new RangeValue<double>(o.Price), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<SampleProduct> DiscountPercentage = new(o => new RangeValue<double>(o.DiscountPercentage), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<SampleProduct> Rating = new(o => new RangeValue<double>(o.Rating), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<SampleProduct> Stock = new(o => new RangeValue<double>(o.Stock), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly KeywordField<SampleProduct> Brand = new(o => new TokenValue(o.Brand), isFilterable: true, isFacetable: true);
        public static readonly KeywordField<SampleProduct> Category = new(o => new TokenValue(o.Category), isFilterable: true, isFacetable: true);

        public IEnumerable<Field> GetFields()
        {
            yield return Text;
            yield return Price;
            yield return DiscountPercentage;
            yield return Rating;
            yield return Stock;
            yield return Brand;
            yield return Category;
        }

        public string GetDocumentKey(SampleProduct item)
        {
            return item.Id.ToString();
        }

        public SampleProduct GetDocument(SampleProduct item)
        {
            return item;
        }
    }
}
