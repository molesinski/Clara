using System.Globalization;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class RangeSortTests
    {
        [Theory]
        [InlineData(SortDirection.Ascending, 0)]
        [InlineData(SortDirection.Descending, 0)]
        [InlineData(SortDirection.Ascending, 1)]
        [InlineData(SortDirection.Descending, 1)]
        [InlineData(SortDirection.Ascending, 3)]
        [InlineData(SortDirection.Descending, 3)]
        [InlineData(SortDirection.Ascending, 32)]
        [InlineData(SortDirection.Descending, 32)]
        public void CustomRangeSortAcceptsExtremeComparisonResults(SortDirection direction, int count)
        {
            var mapper = new RangeMapper();
            var documents = Enumerable.Range(0, count)
                .Select(i => new RangeDocument(i.ToString(CultureInfo.InvariantCulture), i / 2))
                .ToArray();
            var index = IndexBuilder.Build(documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Sort(mapper.Value, direction));

            var values = documents.Select(x => x.Values[0].Value);
            var expected = direction == SortDirection.Descending ? values.OrderByDescending(x => x) : values.OrderBy(x => x);

            Assert.Equal(expected, result.Documents.Select(x => x.Document.Values[0].Value));
            Assert.Equal(count, result.Documents.Select(x => x.Key).Distinct(StringComparer.Ordinal).Count());
        }

        [Theory]
        [InlineData(SortDirection.Ascending, "spread low middle missing")]
        [InlineData(SortDirection.Descending, "spread middle low missing")]
        public void CustomRangeSortUsesDocumentExtremaAndPlacesMissingValuesLast(SortDirection direction, string expected)
        {
            var mapper = new RangeMapper();
            var documents = new[]
            {
                new RangeDocument("missing"),
                new RangeDocument("low", 2, 3),
                new RangeDocument("middle", 5, 6),
                new RangeDocument("spread", 1, 9),
            };
            var index = IndexBuilder.Build(documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Sort(mapper.Value, direction));

            Assert.Equal(expected, string.Join(" ", result.Documents.Select(x => x.Key)));
        }

        private readonly struct RangeValue : IComparable<RangeValue>
        {
            public RangeValue(int value)
            {
                this.Value = value;
            }

            public int Value { get; }

            public int CompareTo(RangeValue other)
            {
                // IComparable requires only the sign, not a result restricted to -1, 0, or 1.
                return this.Value < other.Value ? int.MinValue : this.Value > other.Value ? int.MaxValue : 0;
            }
        }

        private sealed class RangeDocument
        {
            public RangeDocument(string key, params int[] values)
            {
                this.Key = key;
                this.Values = values.Select(x => new RangeValue(x)).ToArray();
            }

            public string Key { get; }

            public RangeValue[] Values { get; }
        }

        private sealed class RangeMapper : IIndexMapper<RangeDocument>
        {
            public RangeField<RangeDocument, RangeValue> Value { get; } = new(
                x => x.Values,
                new RangeValue(int.MinValue),
                new RangeValue(int.MaxValue),
                isSortable: true);

            public IEnumerable<Field> GetFields()
            {
                yield return this.Value;
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
