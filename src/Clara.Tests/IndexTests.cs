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

            var brand = SampleProduct.Items
                .Select(x => x.Brand)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .First();

            var maxPrice = SampleProduct.Items
                .Max(x => x.Price);

            var query = index.QueryBuilder()
                .Search(SampleProductMapper.Text, allTextSynonym)
                .Filter(SampleProductMapper.Brand, Values.Any(brand))
                .Filter(SampleProductMapper.Price, from: 0, to: maxPrice - 1)
                .Facet(SampleProductMapper.Brand)
                .Facet(SampleProductMapper.Category)
                .Facet(SampleProductMapper.Price)
                .Sort(SampleProductMapper.Price, SortDirection.Descending)
                .Query;

            using var result = index.Query(query);

            var input = new HashSet<int>(
                SampleProduct.Items
                    .Where(x => x.Brand == brand)
                    .Where(x => x.Price >= 0 && x.Price <= maxPrice - 1)
                    .Select(x => x.Id));

            var output = new HashSet<int>(
                result.Documents
                    .Select(x => x.Document.Id));

            Debug.Assert(input.SetEquals(output), "Resulting product sets must be equal");
        }
    }
}
