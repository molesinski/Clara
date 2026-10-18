#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Mapping;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class RangeFilterTests
    {
        [Theory]
        [InlineData(null, null, "high1 high2 low1 low2 middle1 middle2 missing")]
        [InlineData(null, 10, "low1 low2")]
        [InlineData(30, null, "high1 high2")]
        [InlineData(10, 30, "high1 high2 low1 low2 middle1 middle2")]
        [InlineData(20, 20, "middle1 middle2")]
        [InlineData(11, 29, "middle1 middle2")]
        [InlineData(10, 20, "low1 low2 middle1 middle2")]
        [InlineData(20, 30, "high1 high2 middle1 middle2")]
        [InlineData(null, 9, "")]
        [InlineData(31, null, "")]
        [InlineData(11, 19, "")]
        public void BoundsAreInclusiveAndRetainAllDocumentsWithEqualValues(int? from, int? to, string expected)
        {
            var mapper = new RangeMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new RangeDocument("high1", 30),
                    new RangeDocument("low1", 10),
                    new RangeDocument("middle1", 20),
                    new RangeDocument("missing"),
                    new RangeDocument("low2", 10),
                    new RangeDocument("high2", 30),
                    new RangeDocument("middle2", 20),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Scalar, from, to));

            Assert.Equal(expected, string.Join(" ", result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal)));
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(10, 10, true)]
        [InlineData(null, 10, true)]
        [InlineData(10, null, true)]
        [InlineData(null, 9, false)]
        [InlineData(11, null, false)]
        [InlineData(9, 11, true)]
        public void SingleIndexedValueHonorsBounds(int? from, int? to, bool matches)
        {
            var mapper = new RangeMapper();
            var index = IndexBuilder.Build(new[] { new RangeDocument("only", 10) }, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Scalar, from, to));

            if (matches)
            {
                Assert.Equal("only", Assert.Single(result.Documents).Key);
            }
            else
            {
                Assert.Empty(result.Documents);
            }
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, 0)]
        [InlineData(0, null)]
        [InlineData(0, 0)]
        public void EmptyIndexReturnsNoDocuments(int? from, int? to)
        {
            var mapper = new RangeMapper();
            var index = IndexBuilder.Build(Array.Empty<RangeDocument>(), mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Scalar, from, to));

            Assert.Empty(result.Documents);
        }

        [Theory]
        [InlineData(int.MinValue, int.MinValue, "min")]
        [InlineData(int.MaxValue, int.MaxValue, "max")]
        [InlineData(int.MinValue, int.MaxValue, "max min zero")]
        public void ExtremeBoundsIncludeExtremeValues(int from, int to, string expected)
        {
            var mapper = new RangeMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new RangeDocument("max", int.MaxValue),
                    new RangeDocument("min", int.MinValue),
                    new RangeDocument("zero", 0),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Scalar, from, to));

            Assert.Equal(expected, string.Join(" ", result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal)));
        }

        [Theory]
        [InlineData(15, 25, "middle repeated")]
        [InlineData(20, 20, "middle repeated")]
        [InlineData(10, 10, "gap low middle")]
        [InlineData(30, 30, "gap high middle")]
        [InlineData(10, 30, "gap high low middle repeated")]
        [InlineData(null, null, "empty gap high low middle missing repeated")]
        public void MultivaluedFieldMatchesAnActualValueAndReturnsEachDocumentOnce(int? from, int? to, string expected)
        {
            var mapper = new RangeMapper();
            var index = IndexBuilder.Build(
                new[]
                {
                    new RangeDocument("gap", 10, 30),
                    new RangeDocument("middle", 30, 20, 10),
                    new RangeDocument("repeated", 20, 20, 20),
                    new RangeDocument("low", 10),
                    new RangeDocument("high", 30),
                    new RangeDocument("empty"),
                    new RangeDocument("missing", (int[]?)null),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Multiple, from, to));

            Assert.Equal(expected, string.Join(" ", result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal)));
        }

        [Theory]
        [InlineData(10, 30, "max min mixed")]
        [InlineData(null, 10, "min")]
        [InlineData(30, null, "max")]
        [InlineData(20, 20, "mixed")]
        [InlineData(null, null, "above below max min missing mixed")]
        public void IndexingSkipsValuesOutsideDeclaredFieldRange(int? from, int? to, string expected)
        {
            var mapper = new RangeMapper(min: 10, max: 30);
            var index = IndexBuilder.Build(
                new[]
                {
                    new RangeDocument("below", 9),
                    new RangeDocument("above", 31),
                    new RangeDocument("min", 10),
                    new RangeDocument("max", 30),
                    new RangeDocument("mixed", 9, 20, 31),
                    new RangeDocument("missing"),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Multiple, from, to));

            Assert.Equal(expected, string.Join(" ", result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal)));
        }

        [Theory]
        [InlineData(null, 30)]
        [InlineData(10, null)]
        [InlineData(10, 30)]
        public void DocumentsWithoutIndexedValuesDoNotMatchAnActiveFilter(int? from, int? to)
        {
            var mapper = new RangeMapper(min: 10, max: 30);
            var index = IndexBuilder.Build(
                new[]
                {
                    new RangeDocument("empty"),
                    new RangeDocument("missing", (int[]?)null),
                    new RangeDocument("outside", 9, 31),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Filter(mapper.Multiple, from, to));

            Assert.Empty(result.Documents);
        }

        [Theory]
        [InlineData(9, null)]
        [InlineData(31, null)]
        [InlineData(null, 9)]
        [InlineData(null, 31)]
        public void BoundsOutsideDeclaredFieldRangeAreRejected(int? from, int? to)
        {
            var mapper = new RangeMapper(min: 10, max: 30);
            var index = IndexBuilder.Build(new[] { new RangeDocument("middle", 20) }, mapper, new InstanceTokenEncoderStore());

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using var result = index.Query(q => q.Filter(mapper.Scalar, from, to));
            });
        }

        [Fact]
        public void ReversedBoundsAreRejected()
        {
            var mapper = new RangeMapper();
            var index = IndexBuilder.Build(new[] { new RangeDocument("middle", 20) }, mapper, new InstanceTokenEncoderStore());

            Assert.Throws<ArgumentException>(() =>
            {
                using var result = index.Query(q => q.Filter(mapper.Scalar, from: 30, to: 10));
            });
        }

        private sealed class RangeDocument
        {
            public RangeDocument(string key, params int[]? values)
            {
                this.Key = key;
                this.Values = values;
            }

            public string Key { get; }

            public int[]? Values { get; }

            public int? Value
            {
                get
                {
                    return this.Values is { Length: > 0 } ? this.Values[0] : null;
                }
            }
        }

        private sealed class RangeMapper : IIndexMapper<RangeDocument>
        {
            public RangeMapper(int min = int.MinValue, int max = int.MaxValue)
            {
                this.Scalar = new(x => x.Value, min, max, isFilterable: true);
                this.Multiple = new(x => x.Values, min, max, isFilterable: true);
            }

            public RangeField<RangeDocument, int> Scalar { get; }

            public RangeField<RangeDocument, int> Multiple { get; }

            public IEnumerable<Field> GetFields()
            {
                yield return this.Scalar;
                yield return this.Multiple;
            }

            public string GetDocumentKey(RangeDocument item)
            {
                return item.Key;
            }

            public RangeDocument GetDocument(RangeDocument item)
            {
                return item;
            }
        }
    }
}
