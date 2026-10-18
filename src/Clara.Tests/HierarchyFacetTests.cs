#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class HierarchyFacetTests
    {
        [Theory]
        [InlineData(HierarchyValueHandling.Path, false)]
        [InlineData(HierarchyValueHandling.Path, true)]
        [InlineData(HierarchyValueHandling.Identifiers, false)]
        [InlineData(HierarchyValueHandling.Identifiers, true)]
        public void RootCountsDocumentsOnceAndShowsOnlyImmediateChildren(HierarchyValueHandling handling, bool selectRoot)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q =>
            {
                if (selectRoot)
                {
                    q.Filter(mapper.Category, FilterMode.Any, "all");
                }

                q.Facet(mapper.Category);
            });

            Assert.Equal(selectRoot ? 5 : 6, result.Documents.Count);
            var root = Assert.Single(result.Facets.Field(mapper.Category).Values);
            AssertNode(root, "all", 5, ("electronics", 4), ("home", 2));
            Assert.Equal(new[] { "electronics", "home" }, root.Children.Select(x => x.Value));
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path, "electronics/computers", "electronics/computers/laptops", "electronics/computers/desktops")]
        [InlineData(HierarchyValueHandling.Identifiers, "computers", "laptops", "desktops")]
        public void SelectingParentReturnsDescendantsAndImmediateChildren(HierarchyValueHandling handling, string parent, string laptops, string desktops)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q.Filter(mapper.Category, FilterMode.Any, parent).Facet(mapper.Category));

            Assert.Equal(new[] { "laptop", "multi" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            var node = Assert.Single(result.Facets.Field(mapper.Category).Values);
            AssertNode(node, parent, 2, (laptops, 2), (desktops, 1));
            Assert.Equal(new[] { laptops, desktops }, node.Children.Select(x => x.Value));
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path, "electronics/computers/laptops")]
        [InlineData(HierarchyValueHandling.Identifiers, "laptops")]
        public void SelectingLeafReturnsItsCountWithoutChildren(HierarchyValueHandling handling, string leaf)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q.Filter(mapper.Category, FilterMode.Any, leaf).Facet(mapper.Category));

            Assert.Equal(new[] { "laptop", "multi" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            AssertNode(Assert.Single(result.Facets.Field(mapper.Category).Values), leaf, 2);
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path, FilterMode.Any)]
        [InlineData(HierarchyValueHandling.Path, FilterMode.All)]
        [InlineData(HierarchyValueHandling.Identifiers, FilterMode.Any)]
        [InlineData(HierarchyValueHandling.Identifiers, FilterMode.All)]
        public void MultipleSelectedBranchesHonorFilterMode(HierarchyValueHandling handling, FilterMode mode)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q
                .Filter(mapper.Category, mode, "home", "electronics", "home")
                .Facet(mapper.Category));

            var values = result.Facets.Field(mapper.Category).Values;
            Assert.Equal(2, values.Count);
            var electronics = values.Single(x => x.Value == "electronics");
            var home = values.Single(x => x.Value == "home");

            if (mode == FilterMode.Any)
            {
                Assert.Equal(new[] { "home", "laptop", "multi", "parent", "phone" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
                AssertNode(electronics, "electronics", 4, (NodeValue(handling, "electronics", "computers"), 2), (NodeValue(handling, "electronics", "phones"), 1));
                AssertNode(home, "home", 2, (NodeValue(handling, "home", "kitchen"), 1), (NodeValue(handling, "home", "garden"), 1));
                Assert.Equal(new[] { "electronics", "home" }, values.Select(x => x.Value));
            }
            else
            {
                Assert.Equal("multi", Assert.Single(result.Documents).Key);
                AssertNode(electronics, "electronics", 1, (NodeValue(handling, "electronics", "computers"), 1));
                AssertNode(home, "home", 1, (NodeValue(handling, "home", "kitchen"), 1));
            }
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path, "electronics/computers", "electronics/computers/laptops", "electronics/computers/desktops")]
        [InlineData(HierarchyValueHandling.Identifiers, "computers", "laptops", "desktops")]
        public void SelectingAncestorAndDescendantKeepsTheirChildrenSeparate(HierarchyValueHandling handling, string computers, string laptops, string desktops)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q
                .Filter(mapper.Category, FilterMode.All, "electronics", computers)
                .Facet(mapper.Category));

            Assert.Equal(new[] { "laptop", "multi" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            var values = result.Facets.Field(mapper.Category).Values;
            Assert.Equal(2, values.Count);
            AssertNode(values.Single(x => x.Value == "electronics"), "electronics", 2, (computers, 2));
            AssertNode(values.Single(x => x.Value == computers), computers, 2, (laptops, 2), (desktops, 1));
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path, false)]
        [InlineData(HierarchyValueHandling.Path, true)]
        [InlineData(HierarchyValueHandling.Identifiers, false)]
        [InlineData(HierarchyValueHandling.Identifiers, true)]
        public void HierarchyAndKeywordFacetsRespectEachOthersFilters(HierarchyValueHandling handling, bool categoryFirst)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q =>
            {
                if (categoryFirst)
                {
                    q.Filter(mapper.Category, FilterMode.Any, "electronics").Filter(mapper.Brand, FilterMode.Any, "A");
                }
                else
                {
                    q.Filter(mapper.Brand, FilterMode.Any, "A").Filter(mapper.Category, FilterMode.Any, "electronics");
                }

                q.Facet(mapper.Category).Facet(mapper.Brand);
            });

            Assert.Equal(new[] { "laptop", "multi", "parent" }, result.Documents.Select(x => x.Key).OrderBy(x => x, StringComparer.Ordinal));
            AssertNode(Assert.Single(result.Facets.Field(mapper.Category).Values), "electronics", 3, (NodeValue(handling, "electronics", "computers"), 2));
            Assert.Equal(
                new[] { ("A", 3, true), ("B", 1, false) },
                result.Facets.Field(mapper.Brand).Values.Select(x => (x.Value, x.Count, x.IsSelected)));
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path, "electronics/computers")]
        [InlineData(HierarchyValueHandling.Identifiers, "computers")]
        public void SelectedNodeExcludedByAnotherFilterHasZeroCountAndNoChildren(HierarchyValueHandling handling, string selected)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q
                .Filter(mapper.Category, FilterMode.Any, selected)
                .Filter(mapper.Brand, FilterMode.Any, "B")
                .Facet(mapper.Category));

            Assert.Empty(result.Documents);
            AssertNode(Assert.Single(result.Facets.Field(mapper.Category).Values), selected, 0);
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path)]
        [InlineData(HierarchyValueHandling.Identifiers)]
        public void UnknownSelectedNodeReturnsNoDocumentsOrFacetValues(HierarchyValueHandling handling)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q.Filter(mapper.Category, FilterMode.Any, "missing").Facet(mapper.Category));

            Assert.Empty(result.Documents);
            Assert.Empty(result.Facets.Field(mapper.Category).Values);
        }

        [Theory]
        [InlineData(HierarchyValueHandling.Path)]
        [InlineData(HierarchyValueHandling.Identifiers)]
        public void EmptyResultsWithoutSelectionKeepRootWithZeroCount(HierarchyValueHandling handling)
        {
            var mapper = new HierarchyMapper(handling);
            var index = CreateIndex(mapper);

            using var result = index.Query(q => q.Filter(mapper.Brand, FilterMode.Any, "missing").Facet(mapper.Category));

            Assert.Empty(result.Documents);
            AssertNode(Assert.Single(result.Facets.Field(mapper.Category).Values), "all", 0);
        }

        [Fact]
        public void PathHandlingKeepsEqualNodeNamesUnderDifferentParentsSeparate()
        {
            var mapper = new HierarchyMapper(HierarchyValueHandling.Path);
            var index = IndexBuilder.Build(
                new[]
                {
                    new HierarchyDocument("home", "A", "home/shared"),
                    new HierarchyDocument("office", "A", "office/shared"),
                },
                mapper,
                new InstanceTokenEncoderStore());

            using var branches = index.Query(q => q.Filter(mapper.Category, FilterMode.Any, "home", "office").Facet(mapper.Category));

            Assert.Equal(2, branches.Documents.Count);
            var values = branches.Facets.Field(mapper.Category).Values;
            Assert.Equal(2, values.Count);
            AssertNode(values.Single(x => x.Value == "home"), "home", 1, ("home/shared", 1));
            AssertNode(values.Single(x => x.Value == "office"), "office", 1, ("office/shared", 1));

            using var leaf = index.Query(q => q.Filter(mapper.Category, FilterMode.Any, "home/shared").Facet(mapper.Category));

            Assert.Equal("home", Assert.Single(leaf.Documents).Key);
            AssertNode(Assert.Single(leaf.Facets.Field(mapper.Category).Values), "home/shared", 1);
        }

        private static Index<HierarchyDocument> CreateIndex(HierarchyMapper mapper)
        {
            return IndexBuilder.Build(
                new[]
                {
                    new HierarchyDocument("multi", "A", "electronics/computers/laptops", "electronics/computers/desktops", "electronics/computers/laptops", "home/kitchen"),
                    new HierarchyDocument("laptop", "A", "electronics/computers/laptops"),
                    new HierarchyDocument("phone", "B", "electronics/phones"),
                    new HierarchyDocument("home", "B", "home/garden"),
                    new HierarchyDocument("parent", "A", "electronics"),
                    new HierarchyDocument("uncategorized", "A", null, string.Empty, " "),
                },
                mapper,
                new InstanceTokenEncoderStore());
        }

        private static string NodeValue(HierarchyValueHandling handling, string parent, string child)
        {
            return handling == HierarchyValueHandling.Path ? parent + "/" + child : child;
        }

        private static void AssertNode(HierarchyFacetValue node, string value, int count, params (string Value, int Count)[] children)
        {
            Assert.Equal(value, node.Value);
            Assert.Equal(count, node.Count);
            Assert.Equal(
                children.OrderBy(x => x.Value, StringComparer.Ordinal),
                node.Children.Select(x => (x.Value, x.Count)).OrderBy(x => x.Value, StringComparer.Ordinal));
            Assert.All(node.Children, child => Assert.Empty(child.Children));
        }

        private sealed class HierarchyDocument
        {
            public HierarchyDocument(string key, string brand, params string?[] categories)
            {
                this.Key = key;
                this.Brand = brand;
                this.Categories = categories;
            }

            public string Key { get; }

            public string Brand { get; }

            public string?[] Categories { get; }
        }

        private sealed class HierarchyMapper : IIndexMapper<HierarchyDocument>
        {
            public HierarchyMapper(HierarchyValueHandling handling)
            {
                this.Category = new(x => x.Categories, separator: "/", root: "all", valueHandling: handling, isFilterable: true, isFacetable: true);
            }

            public HierarchyField<HierarchyDocument> Category { get; }

            public KeywordField<HierarchyDocument> Brand { get; } = new(x => x.Brand, isFilterable: true, isFacetable: true);

            public IEnumerable<Field> GetFields()
            {
                yield return this.Category;
                yield return this.Brand;
            }

            public string GetDocumentKey(HierarchyDocument item)
            {
                return item.Key;
            }

            public HierarchyDocument GetDocument(HierarchyDocument item)
            {
                return item;
            }
        }
    }
}
