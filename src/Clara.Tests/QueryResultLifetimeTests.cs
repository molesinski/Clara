#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class QueryResultLifetimeTests
    {
        [Fact]
        public void DisposingResultAllowsCallerOwnedQueryToBeExecutedAgain()
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var builder = index.QueryBuilder();
            Configure(builder, mapper, 0);
            using var query = builder.ToQuery();
            ResultSnapshot expected;
            using (var first = index.Query(query))
            {
                Assert.Equal(new[] { "2", "1" }, first.Documents.Select(x => x.Key));
                expected = new ResultSnapshot(first, mapper);
            }

            using (var other = Execute(index, mapper, 1))
            {
                Assert.Equal("5", Assert.Single(other.Documents).Key);
            }

            using var repeated = index.Query(query);
            expected.AssertMatches(repeated, mapper);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ResultRemainsUsableAfterQueryAndBuilderAreDisposed(bool explicitQuery)
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var result = ExecuteOwnedQuery(index, mapper, explicitQuery);
            Assert.Equal(new[] { "2", "1" }, result.Documents.Select(x => x.Key));
            var expected = new ResultSnapshot(result, mapper);

            for (var variant = 0; variant < 4; variant++)
            {
                using var other = Execute(index, mapper, variant);
                expected.AssertMatches(result, mapper);
            }

            expected.AssertMatches(result, mapper);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DisposingOneResultAndReusingBuffersDoesNotChangeOtherResults(bool disposeFirst)
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var first = Execute(index, mapper, 0);
            using var second = Execute(index, mapper, 1);
            var disposed = disposeFirst ? first : second;
            var survivor = disposeFirst ? second : first;
            var expected = new ResultSnapshot(survivor, mapper);

            disposed.Dispose();
            using var replacement = Execute(index, mapper, 2);
            Assert.Equal("4", Assert.Single(replacement.Documents).Key);
            var replacementExpected = new ResultSnapshot(replacement, mapper);

            // A second Dispose must not return or reset buffers now owned by another result.
            disposed.Dispose();
            expected.AssertMatches(survivor, mapper);
            replacementExpected.AssertMatches(replacement, mapper);

            for (var variant = 0; variant < 4; variant++)
            {
                using var other = Execute(index, mapper, variant);
                expected.AssertMatches(survivor, mapper);
                replacementExpected.AssertMatches(replacement, mapper);
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DisposingResultInvalidatesOwnedCollectionsEvenWhenEmpty(bool empty)
        {
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            using var result = Execute(index, mapper, empty ? 3 : 0);
            var documents = result.Documents;
            var facets = result.Facets;
            var brand = facets.Field(mapper.Brand);
            var brands = brand.Values;
            var category = facets.Field(mapper.Category);
            var categories = category.Values;
            Assert.Equal(empty ? 0 : 2, documents.Count);

            result.Dispose();

            Assert.Throws<ObjectDisposedException>(() => result.Documents);
            Assert.Throws<ObjectDisposedException>(() => result.Facets);
            Assert.Throws<ObjectDisposedException>(() => documents.Count);
            Assert.Throws<ObjectDisposedException>(() => documents.GetEnumerator());
            Assert.Throws<ObjectDisposedException>(() => facets.Count);
            Assert.Throws<ObjectDisposedException>(() => facets.Field(mapper.Brand));
            Assert.Throws<ObjectDisposedException>(() => brand.Values);
            Assert.Throws<ObjectDisposedException>(() => brands.Count);
            Assert.Throws<ObjectDisposedException>(() => category.Values);
            Assert.Throws<ObjectDisposedException>(() => categories.Count);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task ConcurrentQueriesMatchSequentialResultsAndIsolateFailures(bool failOneWorker)
        {
            const int workerCount = 4;
            const int rounds = 8;
            var mapper = CreateMapper();
            var index = CreateIndex(mapper);
            var expected = Enumerable.Range(0, workerCount).Select(variant =>
            {
                using var result = Execute(index, mapper, variant);
                Assert.Equal(variant == 0 ? 2 : variant == 3 ? 0 : 1, result.Documents.Count);
                return new ResultSnapshot(result, mapper);
            }).ToArray();
            using var barrier = new Barrier(workerCount);
            using var cancellation = new CancellationTokenSource();
            var workers = Enumerable.Range(0, workerCount).Select(worker => Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        for (var round = 0; round < rounds; round++)
                        {
                            var variant = (worker + round) % workerCount;
                            var entered = false;
                            var error = new InvalidOperationException("Scoring failed.");
                            float PositionBoost(Position position)
                            {
                                if (!entered)
                                {
                                    entered = true;

                                    // All queries hold their own analysis state at this point.
                                    Synchronize(barrier, cancellation.Token);
                                    if (failOneWorker && worker == 0)
                                    {
                                        throw error;
                                    }
                                }

                                return 1f;
                            }

                            QueryResult<Product>? result = null;
                            try
                            {
                                if (failOneWorker && worker == 0)
                                {
                                    Assert.Same(error, Assert.Throws<InvalidOperationException>(() =>
                                    {
                                        using var failed = Execute(index, mapper, variant, PositionBoost);
                                    }));
                                }
                                else
                                {
                                    result = Execute(index, mapper, variant, PositionBoost);
                                }

                                // Keep successful results alive together, including after failure cleanup.
                                Synchronize(barrier, cancellation.Token);
                                if (result is not null)
                                {
                                    expected[variant].AssertMatches(result, mapper);
                                }

                                Synchronize(barrier, cancellation.Token);
                            }
                            finally
                            {
                                result?.Dispose();
                            }

                            Synchronize(barrier, cancellation.Token);
                        }
                    }
                    catch
                    {
                        cancellation.Cancel();
                        throw;
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default)).ToArray();

            await Task.WhenAll(workers);

            for (var variant = 0; variant < workerCount; variant++)
            {
                using var result = Execute(index, mapper, variant);
                expected[variant].AssertMatches(result, mapper);
            }
        }

        private static QueryResult<Product> ExecuteOwnedQuery(Index<Product> index, ProductMapper mapper, bool explicitQuery)
        {
            using var builder = index.QueryBuilder();
            Configure(builder, mapper, 0);
            if (explicitQuery)
            {
                using var query = builder.ToQuery();
                return index.Query(query);
            }

            return index.Query(builder);
        }

        private static QueryResult<Product> Execute(Index<Product> index, ProductMapper mapper, int variant, Func<Position, float>? positionBoost = null)
        {
            return index.Query(q => Configure(q, mapper, variant, positionBoost));
        }

        private static void Configure(QueryBuilder builder, ProductMapper mapper, int variant, Func<Position, float>? positionBoost = null)
        {
            var text = variant == 0 ? "red blue" : variant == 1 ? "alias" : variant == 2 ? "green" : "red";
            var brand = variant == 0 ? "A" : variant == 1 ? "C" : "B";
            builder
                .Search(mapper.Text, variant < 2 ? SearchMode.Any : SearchMode.All, text, positionBoost)
                .Filter(mapper.Brand, FilterMode.Any, brand)
                .Filter(mapper.Category, FilterMode.Any, variant == 2 ? "goods-green" : "goods-red")
                .Filter(mapper.Price, from: null, to: variant == 0 ? 25m : 60m)
                .Facet(mapper.Brand)
                .Facet(mapper.Category)
                .Facet(mapper.Price);

            if (variant < 2)
            {
                builder.Sort(mapper.Price, variant == 0 ? SortDirection.Descending : SortDirection.Ascending);
            }
        }

        private static ProductMapper CreateMapper()
        {
            return new ProductMapper(synonyms: new[] { new ExplicitMappingSynonym(new[] { "alias" }, new[] { "red blue" }) });
        }

        private static Index<Product> CreateIndex(ProductMapper mapper)
        {
            return IndexBuilder.Build(
                new[]
                {
                    new Product { Id = 1, Title = "red blue", Brand = "A", Category = "goods-red-small", Price = 10 },
                    new Product { Id = 2, Title = "red", Brand = "A", Category = "goods-red-large", Price = 20 },
                    new Product { Id = 3, Title = "blue", Brand = "B", Category = "goods-blue-small", Price = 30 },
                    new Product { Id = 4, Title = "green", Brand = "B", Category = "goods-green-large", Price = 40 },
                    new Product { Id = 5, Title = "red blue", Brand = "C", Category = "goods-red-small", Price = 50 },
                    new Product { Id = 6, Title = "green", Brand = "C", Category = "goods-green-small", Price = 60 },
                },
                mapper,
                new InstanceTokenEncoderStore());
        }

        private static void Synchronize(Barrier barrier, CancellationToken cancellationToken)
        {
            if (!barrier.SignalAndWait(TimeSpan.FromSeconds(30), cancellationToken))
            {
                throw new TimeoutException("Concurrent queries did not reach their next phase.");
            }
        }

        private sealed class ResultSnapshot
        {
            private readonly (string Key, Product Document, float Score)[] documents;
            private readonly (string Value, int Count, bool IsSelected)[] brands;
            private readonly (string Value, int Count)[] categories;
            private readonly (string Parent, string Value, int Count)[] children;
            private readonly decimal minPrice;
            private readonly decimal maxPrice;

            public ResultSnapshot(QueryResult<Product> result, ProductMapper mapper)
            {
                this.documents = result.Documents.Select(x => (x.Key, x.Document, x.Score)).ToArray();
                this.brands = result.Facets.Field(mapper.Brand).Values
                    .Select(x => (x.Value, x.Count, x.IsSelected)).OrderBy(x => x.Value, StringComparer.Ordinal).ToArray();
                var hierarchy = result.Facets.Field(mapper.Category).Values;
                this.categories = hierarchy.Select(x => (x.Value, x.Count)).OrderBy(x => x.Value, StringComparer.Ordinal).ToArray();

                // Copy children as scalars: HierarchyFacetValue itself retains a pooled children view.
                this.children = hierarchy.SelectMany(parent => parent.Children.Select(child => (Parent: parent.Value, child.Value, child.Count)))
                    .OrderBy(x => x.Parent, StringComparer.Ordinal).ThenBy(x => x.Value, StringComparer.Ordinal).ToArray();
                this.minPrice = result.Facets.Field(mapper.Price).MinValue;
                this.maxPrice = result.Facets.Field(mapper.Price).MaxValue;
            }

            public void AssertMatches(QueryResult<Product> result, ProductMapper mapper)
            {
                var actual = new ResultSnapshot(result, mapper);
                Assert.Equal(this.documents, actual.documents);
                Assert.Equal(this.brands, actual.brands);
                Assert.Equal(this.categories, actual.categories);
                Assert.Equal(this.children, actual.children);
                Assert.Equal(this.minPrice, actual.minPrice);
                Assert.Equal(this.maxPrice, actual.maxPrice);
            }
        }
    }
}
