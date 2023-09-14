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
                var builder =
                    new IndexBuilder<SampleProduct, SampleProduct>(
                        new SampleProductMapper(),
                        Enumerable.Empty<ISynonymMap>(),
                        tokenEncoderStore);

                foreach (var item in SampleProduct.Items)
                {
                    builder.Index(item);
                }

                var index = builder.Build();

                for (var j = 0; j < 3; j++)
                {
                    var brand = SampleProduct.Items.First().Brand;

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

        [Fact]
        public void Search()
        {
            var tokenEncoderStore = new SharedTokenEncoderStore();

            var synonymMap = new SynonymMap(
                SampleProductMapper.Text,
                new Synonym[]
                {
                    new EquivalencySynonym(new[] { "apple", "samsung" }),
                    //// new ExplicitMappingSynonym(new[] { "samsung" }, "apple"),
                    //// new ExplicitMappingSynonym(new[] { "apple" }, "samsung"),
                });

            var builder =
                new IndexBuilder<SampleProduct, SampleProduct>(
                    new SampleProductMapper(),
                    new[] { synonymMap },
                    tokenEncoderStore);

            foreach (var item in SampleProduct.Items)
            {
                builder.Index(item);
            }

            var index = builder.Build();

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(SampleProductMapper.Text, "apple smartphone")
                    .Query);

            var documents = result.Documents
                .Select(x => new DocumentResultView(x))
                .ToList();
        }

        private record struct DocumentResultView
        {
            public DocumentResultView(DocumentResult<SampleProduct> product)
            {
                this.Key = product.Key;
                this.Score = product.Score;
                this.Text = SampleProductMapper.ToText(product.Document);
            }

            public string Key { get; }

            public float Score { get; }

            public string Text { get; }
        }
    }
}
