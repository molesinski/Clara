#pragma warning disable SA1516 // Elements should be separated by blank line

using System.Globalization;
using System.Text;
using Clara.Analysis;
using Clara.Mapping;

namespace Clara.Benchmarks
{
    public class ProductMapper : IIndexMapper<Product>
    {
        public static TextField<Product> Text { get; } = new(x => GetText(x), new PorterAnalyzer());
        public static DecimalField<Product> Price { get; } = new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);
        public static DoubleField<Product> DiscountPercentage { get; } = new(x => x.DiscountPercentage, isFilterable: true, isFacetable: true, isSortable: true);
        public static DoubleField<Product> Rating { get; } = new(x => x.Rating, isFilterable: true, isFacetable: true, isSortable: true);
        public static Int32Field<Product> Stock { get; } = new(x => x.Stock, isFilterable: true, isFacetable: true, isSortable: true);
        public static KeywordField<Product> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);
        public static KeywordField<Product> Category { get; } = new(x => x.Category, isFilterable: true, isFacetable: true);

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

        private static string GetText(Product product)
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
