using System.Globalization;
using System.Text;
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
                new PorterPossessiveTokenFilter(),
                new KeywordLengthTokenFilter(),
                new KeywordDigitsTokenFilter(),
                new PorterStopTokenFilter(),
                new PorterStemTokenFilter());

        public static readonly TextField<SampleProduct> Text = new(ToText, Analyzer);
        public static readonly DoubleField<SampleProduct> Price = new(o => new RangeValue<double>(o.Price), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<SampleProduct> DiscountPercentage = new(o => new RangeValue<double>(o.DiscountPercentage), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<SampleProduct> Rating = new(o => new RangeValue<double>(o.Rating), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<SampleProduct> Stock = new(o => new RangeValue<double>(o.Stock), isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly KeywordField<SampleProduct> Brand = new(o => new TokenValue(o.Brand), isFilterable: true, isFacetable: true);
        public static readonly KeywordField<SampleProduct> Category = new(o => new TokenValue(o.Category), isFilterable: true, isFacetable: true);

        public static string AllText { get; } = Guid.NewGuid().ToString("N");

        public static string ToText(SampleProduct product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var builder = new StringBuilder();

            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}", product.Id);
            builder.AppendLine();
            builder.Append(product.Title);
            builder.AppendLine();
            builder.Append(product.Description);
            builder.AppendLine();
            builder.Append(product.Brand);
            builder.AppendLine();
            builder.Append(product.Category);
            builder.AppendLine();
            builder.Append(AllText);

            return builder.ToString();
        }

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
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.Id.ToString(CultureInfo.InvariantCulture);
        }

        public SampleProduct GetDocument(SampleProduct item)
        {
            return item;
        }
    }
}
