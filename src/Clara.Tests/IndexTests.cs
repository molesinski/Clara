using System.Diagnostics;
using Clara.Analysis.Synonyms;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class IndexTests
    {
        [Fact]
        public void IndexAndQuery()
        {
            var allTextSynonym = Guid.NewGuid().ToString("N");

            var topBrand = SampleProduct.Items
                .Select(x => x.Brand)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            var maxPrice = SampleProduct.Items
                .Max(x => x.Price);

            var synonymMap =
                new SynonymMap(
                    SampleProductMapper.Text,
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { SampleProductMapper.AllText, allTextSynonym }),
                    });

            var builder =
                new IndexBuilder<SampleProduct, SampleProduct>(
                    new SampleProductMapper(),
                    new[] { synonymMap });

            foreach (var item in SampleProduct.Items)
            {
                builder.Index(item);
            }

            var index = builder.Build();

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(SampleProductMapper.Text, allTextSynonym)
                    .Filter(SampleProductMapper.Brand, Values.Any(topBrand))
                    .Filter(SampleProductMapper.Price, from: 1, to: maxPrice - 1)
                    .Facet(SampleProductMapper.Brand)
                    .Facet(SampleProductMapper.Category)
                    .Facet(SampleProductMapper.Price)
                    .Sort(SampleProductMapper.Price, SortDirection.Descending));

            var input = new HashSet<int>(
                SampleProduct.Items
                    .Where(x => x.Brand == topBrand)
                    .Where(x => x.Price >= 1 && x.Price <= maxPrice - 1)
                    .Select(x => x.Id));

            var output = new HashSet<int>(
                result.Documents
                    .Select(x => x.Document.Id));

            Debug.Assert(input.SetEquals(output), "Resulting product sets must be equal");
        }
    }
}
