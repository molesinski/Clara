using System.Diagnostics;
using Clara.Analysis.Synonyms;
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
            var tokenEncoderStore = new SharedTokenEncoderStore();

            for (var i = 0; i < 3; i++)
            {
                var index = default(Index<SampleProduct>);

                var builder =
                    new IndexBuilder<SampleProduct, SampleProduct>(
                        new SampleProductMapper(),
                        Enumerable.Empty<ISynonymMap>(),
                        tokenEncoderStore);

                foreach (var item in SampleProduct.Items)
                {
                    builder.Index(item);
                }

                index = builder.Build();

                for (var j = 0; j < 3; j++)
                {
                    var brand = SampleProduct.Items.First().Brand;

                    var queryBuilder = index.QueryBuilder()
                        .Filter(SampleProductMapper.Brand, Match.Any(brand));

                    using var result = index.Query(queryBuilder.Query);

                    var input = new HashSet<SampleProduct>(SampleProduct.Items.Where(x => x.Brand == brand));
                    var output = new HashSet<SampleProduct>(result.Documents.Select(x => x.Document));

                    Debug.Assert(input.SetEquals(output), "Resulting product sets must be equal");
                }
            }
        }
    }
}
