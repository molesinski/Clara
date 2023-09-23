using System.Globalization;
using System.Text;
using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Benchmarks
{
    public class ProductMapper : IIndexMapper<Product>
    {
        public static readonly TextField<Product> Text = new(ToText, new PorterAnalyzer());
        public static readonly DecimalField<Product> Price = new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<Product> DiscountPercentage = new(x => x.DiscountPercentage, isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<Product> Rating = new(x => x.Rating, isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly DoubleField<Product> Stock = new(x => x.Stock, isFilterable: true, isFacetable: true, isSortable: true);
        public static readonly KeywordField<Product> Brand = new(x => x.Brand, isFilterable: true, isFacetable: true);
        public static readonly KeywordField<Product> Category = new(x => x.Category, isFilterable: true, isFacetable: true);

        public static string CommonTextPhrase { get; } = Guid.NewGuid().ToString("N");

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

        public string GetDocumentKey(Product item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.Id.ToString(CultureInfo.InvariantCulture);
        }

        public Product GetDocument(Product item)
        {
            return item;
        }

        private static string ToText(Product product)
        {
            var builder = new StringBuilder();

            builder.AppendLine(product.Id.ToString(CultureInfo.InvariantCulture));
            builder.AppendLine(product.Title);
            builder.AppendLine(product.Description);
            builder.AppendLine(product.Brand);
            builder.AppendLine(product.Category);
            builder.AppendLine(CommonTextPhrase);

            return builder.ToString();
        }
    }
}
