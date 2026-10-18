#pragma warning disable CA1861 // Avoid constant arrays as arguments

using System.Reflection;
using Clara.Analysis;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    // Pool snapshots require exclusive access to the process-wide pools.
    [CollectionDefinition(nameof(PoolOwnershipTests), DisableParallelization = true)]
    [Collection(nameof(PoolOwnershipTests))]
    public class PoolOwnershipTests
    {
        [Theory]
        [InlineData(SearchMode.Any, false, 1)]
        [InlineData(SearchMode.Any, false, 2)]
        [InlineData(SearchMode.All, false, 2)]
        [InlineData(SearchMode.Any, true, 2)]
        [InlineData(SearchMode.All, true, 2)]
        public void PositionBoostFailureReturnsSearchAndBuilderBuffers(SearchMode mode, bool synonyms, int failAt)
        {
            var mapper = CreateMapper(synonyms);
            var index = CreateIndex(mapper);
            using var query = CreateQuery(index, mapper);
            using (index.Query(query))
            {
            }

            var before = SnapshotPools("DocumentScores", "DocumentSets");
            var error = new InvalidOperationException("Position boost failed.");

            for (var attempt = 0; attempt < 3; attempt++)
            {
                var calls = 0;
                query.Search = new TextSearchExpression(mapper.Text, mode, synonyms ? "alias red" : "red blue", _ => ++calls == failAt ? throw error : 1f);

                Assert.Same(error, Assert.Throws<InvalidOperationException>(() => index.Query(query)));
                Assert.Equal(failAt, calls);
                AssertPoolsReturned(before);
            }

            query.Search = new TextSearchExpression(mapper.Text, SearchMode.All, "red blue");
            using var result = index.Query(query);
            Assert.Equal("1", Assert.Single(result.Documents).Key);
        }

        [Theory]
        [InlineData(SearchMode.Any, false)]
        [InlineData(SearchMode.All, false)]
        [InlineData(SearchMode.Any, true)]
        [InlineData(SearchMode.All, true)]
        public void AggregationFailureReturnsSearchBuffers(SearchMode mode, bool synonyms)
        {
            var mapper = CreateMapper(synonyms);
            var index = CreateIndex(mapper);
            using var query = CreateQuery(index, mapper);
            using (index.Query(query))
            {
            }

            var before = SnapshotPools("DocumentScores");
            var error = new InvalidOperationException("Aggregation failed.");
            query.Search = new TextSearchExpression(mapper.Text, mode, synonyms ? "alias" : "red blue", searchScoreAggregation: new ThrowingAggregation(error));

            for (var attempt = 0; attempt < 3; attempt++)
            {
                Assert.Same(error, Assert.Throws<InvalidOperationException>(() => index.Query(query)));
                AssertPoolsReturned(before);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SelectionFailureReturnsBuffers(bool exclude)
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var query = CreateQuery(index, mapper);
            using (index.Query(query))
            {
            }

            var before = SnapshotPools("DocumentSets", "DocumentScores", "QueryResultBuilderFacets");
            var error = new InvalidOperationException("Selection failed.");

            if (exclude)
            {
                query.ExcludeDocuments = ThrowAfterOne("1", error);
            }
            else
            {
                query.IncludeDocuments = ThrowAfterOne("1", error);
            }

            for (var attempt = 0; attempt < 3; attempt++)
            {
                Assert.Same(error, Assert.Throws<InvalidOperationException>(() => index.Query(query)));
                AssertPoolsReturned(before);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void LaterFailureDisposesCompletedFacets(bool sorting)
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var query = CreateQuery(index, mapper);
            using (index.Query(query))
            {
            }

            var foreignMapper = CreateMapper();

            if (sorting)
            {
                query.Sort = new RangeSortExpression<decimal>(foreignMapper.Price, SortDirection.Ascending);
            }
            else
            {
                query.AddFacet(new KeywordFacetExpression(foreignMapper.Brand));
            }

            var before = SnapshotPools("DocumentScores", "DocumentSets", "QueryResultBuilderFacets", "FacetResults", "KeywordFacetValues", "HierarchyFacetValues");

            for (var attempt = 0; attempt < 3; attempt++)
            {
                Assert.Throws<InvalidOperationException>(() => index.Query(query));
                AssertPoolsReturned(before);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void FilterConstructionFailureReturnsValues(bool hierarchy)
        {
            var mapper = CreateMapper();
            using (new KeywordFilterExpression(mapper.Brand, FilterMode.Any, "A"))
            {
            }

            var before = SnapshotPools("FilterValues");
            var error = new InvalidOperationException("Filter values failed.");

            for (var attempt = 0; attempt < 3; attempt++)
            {
                Assert.Same(error, Assert.Throws<InvalidOperationException>(() =>
                {
                    using FilterExpression filter = hierarchy
                        ? new HierarchyFilterExpression(mapper.Category, FilterMode.Any, ThrowAfterOne("all", error))
                        : new KeywordFilterExpression(mapper.Brand, FilterMode.Any, ThrowAfterOne("A", error));
                }));
                AssertPoolsReturned(before);
            }
        }

        [Fact]
        public void ReplacingFilterDisposesPreviousOwnerButReaddingSameFilterDoesNot()
        {
            var mapper = CreateMapper();
            using var query = new Query(CreateIndex(mapper));
            using var previous = new KeywordFilterExpression(mapper.Brand, FilterMode.Any, "A");
            using var replacement = new KeywordFilterExpression(mapper.Brand, FilterMode.Any, "B");

            query.AddFilter(previous);
            query.AddFilter(previous);
            Assert.Equal("A", Assert.Single(previous.Values));

            query.AddFilter(replacement);
            Assert.Throws<ObjectDisposedException>(() => previous.Values);
            Assert.Same(replacement, Assert.Single(query.Filters));
            Assert.Equal("B", Assert.Single(replacement.Values));

            query.Dispose();
            Assert.Throws<ObjectDisposedException>(() => replacement.Values);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SuccessfulResultReturnsOutputBuffersWhenDisposed(bool sorted)
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var query = CreateQuery(index, mapper);

            if (sorted)
            {
                query.Sort = new RangeSortExpression<decimal>(mapper.Price, SortDirection.Ascending);
            }

            using (index.Query(query))
            {
            }

            var before = SnapshotPools("DocumentScores", "Documents", "FacetResults", "KeywordFacetValues", "HierarchyFacetValues");
            using (var result = index.Query(query))
            {
                Assert.Equal("1", Assert.Single(result.Documents).Key);
            }

            AssertPoolsReturned(before);
        }

        private static ProductMapper CreateMapper(bool synonyms = false)
        {
            return new ProductMapper(synonyms: synonyms ? new[] { new ExplicitMappingSynonym(new[] { "alias" }, new[] { "red blue" }) } : null);
        }

        private static Index<Product> CreateIndex(ProductMapper mapper)
        {
            return IndexBuilder.Build(
                new[]
                {
                    new Product { Id = 1, Title = "red blue", Brand = "A", Category = "goods-red", Price = 2 },
                    new Product { Id = 2, Title = "blue", Brand = "B", Category = "goods-blue", Price = 1 },
                },
                mapper,
                new InstanceTokenEncoderStore());
        }

        private static Query CreateQuery(Index<Product> index, ProductMapper mapper)
        {
            using var builder = index.QueryBuilder()
                .Include(new[] { "1", "2" })
                .Search(mapper.Text, SearchMode.Any, "red blue")
                .Filter(mapper.Brand, FilterMode.Any, "A")
                .Facet(mapper.Brand)
                .Facet(mapper.Category);

            return builder.ToQuery();
        }

        private static IEnumerable<string?> ThrowAfterOne(string value, Exception error)
        {
            yield return value;
            throw error;
        }

        private static Dictionary<string, object[]> SnapshotPools(params string[] poolNames)
        {
            // Inspect only pools relevant to the ownership regression being tested.
            var pools = typeof(IndexBuilder).Assembly.GetType("Clara.SharedObjectPools", throwOnError: true)!;
            var snapshot = new Dictionary<string, object[]>(StringComparer.Ordinal);

            foreach (var name in poolNames)
            {
                var property = pools.GetProperty(name, BindingFlags.Public | BindingFlags.Static)!;
                var pool = property.GetValue(null)!;
                var slots = (Array)pool.GetType().GetField("items", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(pool)!;
                var valueField = slots.GetType().GetElementType()!.GetField("Value")!;
                snapshot.Add(property.Name, slots.Cast<object>().Select(valueField.GetValue).Where(value => value is not null).Cast<object>().ToArray());
            }

            return snapshot;
        }

        private static void AssertPoolsReturned(Dictionary<string, object[]> before)
        {
            var after = SnapshotPools(before.Keys.ToArray());

            foreach (var pool in before)
            {
                foreach (var instance in pool.Value)
                {
                    Assert.True(after[pool.Key].Any(value => ReferenceEquals(value, instance)), $"Lost a buffer from {pool.Key}.");
                }

                var instances = after[pool.Key];

                for (var i = 0; i < instances.Length; i++)
                {
                    Assert.DoesNotContain(instances.Skip(i + 1), value => ReferenceEquals(value, instances[i]));
                }
            }
        }

        private sealed class ThrowingAggregation : ScoreAggregation
        {
            private readonly Exception error;

            public ThrowingAggregation(Exception error)
            {
                this.error = error;
            }

            public override float Combine(float current, float value, float valueBoost)
            {
                throw this.error;
            }
        }
    }
}
