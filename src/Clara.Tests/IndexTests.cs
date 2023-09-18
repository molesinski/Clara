using System.Diagnostics;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class IndexTests
    {
        [Fact]
        public void IndexAndQuery()
        {
            var builder =
                new IndexBuilder<SampleProduct, SampleProduct>(
                    new SampleProductMapper());

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

            using var result = index.Query(
                index.QueryBuilder()
                    .Filter(SampleProductMapper.Brand, Values.Any(brand))
                    .Query);

            var input = new HashSet<SampleProduct>(SampleProduct.Items.Where(x => x.Brand == brand));
            var output = new HashSet<SampleProduct>(result.Documents.Select(x => x.Document));

            Debug.Assert(input.SetEquals(output), "Resulting product sets must be equal");
        }
    }
}
