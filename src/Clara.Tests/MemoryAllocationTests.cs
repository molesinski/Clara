using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class MemoryAllocationTests
    {
        private const string AllTextPhrase = "__ALL";
        private const string AllNoneTextPhrase = "__ALL __NONE";

        private readonly string? topBrand;
        private readonly decimal? maxPrice;
        private readonly Index<Product> index;

        public MemoryAllocationTests()
        {
            this.topBrand = Product.Items
                .Select(x => x.Brand)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            this.maxPrice = Product.Items
                .Max(x => x.Price);

            var synonymMapBindings =
                new[]
                {
                    new SynonymMapBinding(
                        new SynonymMap(
                            ProductMapper.Analyzer,
                            new Synonym[]
                            {
                                new EquivalencySynonym(new[] { ProductMapper.CommonTextPhrase, AllTextPhrase }),
                            }),
                        ProductMapper.Text),
                };

            this.index = IndexBuilder.Build(Product.Items, new ProductMapper(), synonymMapBindings);
        }

        [Fact]
        public void QueryComplex()
        {
            for (var i = 0; i < 10; i++)
            {
                using var result = this.index.Query(
                    this.index.QueryBuilder()
                        .Search(ProductMapper.Text, SearchMode.Any, AllNoneTextPhrase)
                        .Filter(ProductMapper.Brand, FilterMode.Any, this.topBrand)
                        .Filter(ProductMapper.Price, from: 1, to: this.maxPrice - 1)
                        .Facet(ProductMapper.Brand)
                        .Facet(ProductMapper.Category)
                        .Facet(ProductMapper.Price)
                        .Sort(ProductMapper.Rating, SortDirection.Descending));

                ConsumeResult(result);
            }
        }

        private static void ConsumeResult(QueryResult<Product> result)
        {
            foreach (var document in result.Documents)
            {
                _ = document;
            }

            foreach (var facet in result.Facets)
            {
                if (facet is KeywordFacetResult keywordFacetResult)
                {
                    foreach (var value in keywordFacetResult.Values)
                    {
                    }
                }
                else if (facet is HierarchyFacetResult hierarchyFacetResult)
                {
                    foreach (var value in hierarchyFacetResult.Values)
                    {
                        foreach (var child in value.Children)
                        {
                            _ = child;
                        }
                    }
                }
                else if (facet is RangeFacetResult<decimal>)
                {
                }
            }
        }
    }
}
