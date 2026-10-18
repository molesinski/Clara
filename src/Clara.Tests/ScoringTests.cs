#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Clara.Utils;
using Xunit;

namespace Clara.Tests
{
    public class ScoringTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(256)]
        public void DefaultDocumentLengthCountsEmittedTokens(int count)
        {
            var similarity = new RecordingSimilarity();
            var mapper = new ScoringMapper(similarity: similarity);
            var document = new ScoringDocument("document", new TextWeight(string.Join(" ", Enumerable.Repeat("red", count))));

            IndexBuilder.Build(new[] { document }, mapper, new InstanceTokenEncoderStore());

            Assert.Equal((float)count, Assert.Single(similarity.Lengths));
        }

        [Theory]
        [InlineData(false, 7f, 4f)]
        [InlineData(true, 3f, 2f)]
        public void DocumentLengthAggregatesWeightsAcrossValues(bool useMax, float expectedLength, float expectedScore)
        {
            var similarity = new RecordingSimilarity();
            var mapper = new ScoringMapper(
                similarity: similarity,
                indexScoreAggregation: useMax ? ScoreAggregation.Max : ScoreAggregation.Sum);
            var document = new ScoringDocument(
                "document",
                new TextWeight("red red", 2f, expandSynonyms: true),
                new TextWeight("blue", 3f, expandSynonyms: true));
            var emptyDocument = new ScoringDocument("empty", new TextWeight(" "));
            var index = IndexBuilder.Build(new[] { document, emptyDocument }, mapper, new InstanceTokenEncoderStore());

            Assert.Equal(expectedLength, Assert.Single(similarity.Lengths));

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red"));

            Assert.Equal(expectedScore, Assert.Single(result.Documents).Score);
        }

        [Theory]
        [InlineData(false, false, 3f)]
        [InlineData(false, true, 5f)]
        [InlineData(true, false, 2f)]
        [InlineData(true, true, 2f)]
        public void DocumentLengthUsesTheSelectedSynonymExpansion(bool useMax, bool expandSynonyms, float expectedLength)
        {
            var similarity = new RecordingSimilarity();
            var mapper = new ScoringMapper(
                similarity: similarity,
                indexScoreAggregation: useMax ? ScoreAggregation.Max : ScoreAggregation.Sum,
                synonyms: new[] { new ExplicitMappingSynonym(new[] { "alias" }, new[] { "red blue" }) });
            var document = new ScoringDocument(
                "document",
                new TextWeight("alias", 2f, expandSynonyms),
                new TextWeight("green", 1f, expandSynonyms: false));

            IndexBuilder.Build(new[] { document }, mapper, new InstanceTokenEncoderStore());

            Assert.Equal(expectedLength, Assert.Single(similarity.Lengths));
        }

        [Fact]
        public void Bm25UsesWeightedTokenLengthsAndExcludesEmptyDocuments()
        {
            var mapper = new ScoringMapper();
            var documents = new[]
            {
                new ScoringDocument("short", new TextWeight("red")),
                new ScoringDocument("long", new TextWeight("red blue blue")),
                new ScoringDocument("empty", new TextWeight(string.Empty)),
            };
            var index = IndexBuilder.Build(documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red"));

            // N = 2, df = 2, average length = 2; the empty document is not part of N.
            Assert.Equal(new[] { "short", "long" }, result.Documents.Select(x => x.Key));
            Assert.Equal(0.22920424f, result.Documents[0].Score, precision: 6);
            Assert.Equal(0.15136129f, result.Documents[1].Score, precision: 6);
        }

        [Fact]
        public void Bm25KeepsLongDocumentScoresFinite()
        {
            var mapper = new ScoringMapper();
            var documents = new[]
            {
                new ScoringDocument("short", new TextWeight("red")),
                new ScoringDocument("long", new TextWeight("red " + string.Join(" ", Enumerable.Repeat("blue", 255)))),
            };
            var index = IndexBuilder.Build(documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red"));

            Assert.Equal(new[] { "short", "long" }, result.Documents.Select(x => x.Key));
            Assert.All(
                result.Documents,
                hit =>
                {
                    Assert.False(float.IsNaN(hit.Score));
                    Assert.False(float.IsInfinity(hit.Score));
                    Assert.True(hit.Score > 0);
                });
            Assert.True(result.Documents[0].Score > result.Documents[1].Score);
        }

        [Theory]
        [InlineData(false, null, 2f)]
        [InlineData(true, null, 1f)]
        [InlineData(false, false, 2f)]
        [InlineData(false, true, 1f)]
        [InlineData(true, false, 2f)]
        [InlineData(true, true, 1f)]
        public void SynonymPhraseUsesEffectiveSearchAggregation(bool fieldMax, bool? queryMax, float expectedScore)
        {
            var mapper = new ScoringMapper(
                similarity: Similarity.TF,
                searchScoreAggregation: fieldMax ? ScoreAggregation.Max : ScoreAggregation.Sum,
                synonyms: new[] { new ExplicitMappingSynonym(new[] { "alias" }, new[] { "red blue" }) });
            var document = new ScoringDocument("document", new TextWeight("red blue"));
            var index = IndexBuilder.Build(new[] { document }, mapper, new InstanceTokenEncoderStore());
            var aggregation = queryMax.HasValue ? (queryMax.Value ? ScoreAggregation.Max : ScoreAggregation.Sum) : null;

            foreach (var mode in new[] { SearchMode.Any, SearchMode.All })
            {
                using var result = index.Query(q => q.Search(mapper.Text, mode, "alias", scoreAggregation: aggregation));

                Assert.Equal(expectedScore, Assert.Single(result.Documents).Score);
            }
        }

        [Theory]
        [InlineData(SearchMode.Any, false, true, 2f, 2)]
        [InlineData(SearchMode.All, false, true, 2f, 1)]
        [InlineData(SearchMode.Any, true, false, 5f, 2)]
        [InlineData(SearchMode.All, true, false, 5f, 1)]
        public void SynonymAggregationComposesWithPositionBoostAndOtherTerms(SearchMode mode, bool fieldMax, bool queryMax, float expectedScore, int expectedCount)
        {
            var mapper = new ScoringMapper(
                similarity: Similarity.TF,
                searchScoreAggregation: fieldMax ? ScoreAggregation.Max : ScoreAggregation.Sum,
                synonyms: new[] { new ExplicitMappingSynonym(new[] { "alias" }, new[] { "red blue" }) });
            var documents = new[]
            {
                new ScoringDocument("complete", new TextWeight("red blue green")),
                new ScoringDocument("partial", new TextWeight("green")),
            };
            var index = IndexBuilder.Build(documents, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(
                mapper.Text,
                mode,
                "alias green",
                positionBoost: position => position.Start == 0 ? 2f : 1f,
                scoreAggregation: queryMax ? ScoreAggregation.Max : ScoreAggregation.Sum));

            Assert.Equal(expectedCount, result.Documents.Count);
            Assert.Equal(expectedScore, result.Documents.Single(x => x.Key == "complete").Score);

            if (mode == SearchMode.Any)
            {
                Assert.Equal(1f, result.Documents.Single(x => x.Key == "partial").Score);
            }
        }

        [Theory]
        [InlineData("red", 0)]
        [InlineData("red blue", 1)]
        [InlineData("red blue green", 1)]
        public void SynonymAlternativesKeepTheMaximumCompletePhraseScore(string text, int expectedCount)
        {
            var mapper = new ScoringMapper(
                similarity: Similarity.TF,
                searchScoreAggregation: ScoreAggregation.Max,
                synonyms: new[] { new ExplicitMappingSynonym(new[] { "alias" }, new[] { "red blue", "red green" }) });
            var document = new ScoringDocument("document", new TextWeight(text));
            var index = IndexBuilder.Build(new[] { document }, mapper, new InstanceTokenEncoderStore());

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.Any, "alias", scoreAggregation: ScoreAggregation.Sum));

            Assert.Equal(expectedCount, result.Documents.Count);

            if (expectedCount > 0)
            {
                Assert.Equal(2f, Assert.Single(result.Documents).Score);
            }
        }

        private sealed class RecordingSimilarity : Similarity
        {
            public IReadOnlyList<float> Lengths { get; private set; } = Array.Empty<float>();

            public override void Transform(DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores, DictionarySlim<int, float> documentLengths)
            {
                this.Lengths = documentLengths.Select(x => x.Value).ToArray();
            }
        }

        private sealed class ScoringDocument
        {
            public ScoringDocument(string key, params TextWeight[] values)
            {
                this.Key = key;
                this.Values = values;
            }

            public string Key { get; }

            public IReadOnlyList<TextWeight> Values { get; }
        }

        private sealed class ScoringMapper : IIndexMapper<ScoringDocument>
        {
            public ScoringMapper(
                Similarity? similarity = null,
                ScoreAggregation? indexScoreAggregation = null,
                ScoreAggregation? searchScoreAggregation = null,
                IEnumerable<Synonym>? synonyms = null)
            {
                var analyzer = new StandardAnalyzer();
                var synonymMap = synonyms is null ? null : new SynonymMap(analyzer, synonyms);

                this.Text = new TextField<ScoringDocument>(x => x.Values, analyzer, synonymMap, similarity, indexScoreAggregation, searchScoreAggregation);
            }

            public TextField<ScoringDocument> Text { get; }

            public IEnumerable<Field> GetFields()
            {
                yield return this.Text;
            }

            public string GetDocumentKey(ScoringDocument item)
            {
                return item.Key;
            }

            public ScoringDocument GetDocument(ScoringDocument item)
            {
                return item;
            }
        }
    }
}
