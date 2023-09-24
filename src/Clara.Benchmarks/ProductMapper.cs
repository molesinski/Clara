#pragma warning disable SA1516 // Elements should be separated by blank line

using System.Globalization;
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

        private static IEnumerable<string?> GetText(Product product)
        {
            yield return product.Id.ToString(CultureInfo.InvariantCulture);
            yield return product.Title;
            yield return product.Description;
            yield return product.Brand;
            yield return product.Category;
            yield return CommonTextPhrase;
        }
    }
}
