using System.Diagnostics;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class IndexTests
    {
        [Fact]
        public void IndexAndQuery()
        {
            var brand = SampleProduct.Data.First().Brand;

            using var tokenEncoderStore = new SharedTokenEncoderStore();

            for (var i = 0; i < 3; i++)
            {
                var index = default(Index<SampleProduct>);

                using (var builder = new IndexBuilder<SampleProduct, SampleProduct>(new SampleProductMapper()))
                {
                    foreach (var item in SampleProduct.Data)
                    {
                        builder.Index(item);
                    }

                    index = builder.Build();
                }

                for (var j = 0; j < 3; j++)
                {
                    var queryBuilder = index.QueryBuilder()
                        .Filter(SampleProductMapper.Brand, Match.Any(brand));

                    using var result = index.Query(queryBuilder.Query);

                    var input = new HashSet<int>(SampleProduct.Data.Where(x => x.Brand == brand).Select(x => x.Id));
                    var output = new HashSet<int>(result.Documents.Select(x => x.Document).Select(x => x.Id));

                    Debug.Assert(input.SetEquals(output), "Resulting product sets must be equal");
                }
            }
        }
    }
}
