#pragma warning disable SA1516 // Elements should be separated by blank line

using System.Globalization;
using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;

namespace Clara.Tests
{
    public class ProductMapper : IIndexMapper<Product>
    {
        public ProductMapper(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null,
            IEnumerable<Synonym>? synonyms = null)
        {
            var analyzer = new PorterAnalyzer(stopwords: stopwords, keywords: keywords);
            var synonymMap = default(ISynonymMap?);

            if (synonyms is not null)
            {
                synonymMap = new SynonymMap(analyzer, synonyms);
            }

            this.Text = new(GetText, analyzer, synonymMap);
        }

        public static string CommonTextPhrase { get; } = "__COMMON";

        public TextField<Product> Text { get; }
        public DecimalField<Product> Price { get; } = new(x => x.Price, isFilterable: true, isFacetable: true, isSortable: true);
        public DoubleField<Product> DiscountPercentage { get; } = new(x => x.DiscountPercentage, isFilterable: true, isFacetable: true, isSortable: true);
        public DoubleField<Product> Rating { get; } = new(x => x.Rating, isFilterable: true, isFacetable: true, isSortable: true);
        public Int32Field<Product> Stock { get; } = new(x => x.Stock, isFilterable: true, isFacetable: true, isSortable: true);
        public KeywordField<Product> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);
        public HierarchyField<Product> Category { get; } = new(x => x.Category, separator: "-", root: "all", HierarchyValueHandling.Path, isFilterable: true, isFacetable: true);

        public IEnumerable<Field> GetFields()
        {
            yield return this.Text;
            yield return this.Price;
            yield return this.DiscountPercentage;
            yield return this.Rating;
            yield return this.Stock;
            yield return this.Brand;
            yield return this.Category;
        }

        public string GetDocumentKey(Product item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item!.Id.ToString(CultureInfo.InvariantCulture);
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
