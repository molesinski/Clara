#pragma warning disable CA1861 // Avoid constant arrays as arguments

using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Querying;
using Xunit;

namespace Clara.Tests
{
    public class SynonymMapTests
    {
        [Theory]
        //// Baseline
        [InlineData("xxx", SearchMode.All, "", false)]
        [InlineData("xxx", SearchMode.Any, "", false)]
        [InlineData("xxx", SearchMode.All, "xxx", true)]
        [InlineData("xxx", SearchMode.Any, "xxx", true)]
        //// Equivalency : i pad, i-pad, ipad
        [InlineData("i-pad", SearchMode.All, "", false)]
        [InlineData("i-pad", SearchMode.Any, "", false)]
        [InlineData("i-pad", SearchMode.All, "xxx", false)]
        [InlineData("i-pad", SearchMode.Any, "xxx", false)]
        [InlineData("i pad", SearchMode.All, "i pad", true)]
        [InlineData("i pad", SearchMode.Any, "i pad", true)]
        [InlineData("i-pad", SearchMode.All, "i pad", true)]
        [InlineData("i-pad", SearchMode.Any, "i-pad", true)]
        [InlineData("ipad", SearchMode.All, "ipad", true)]
        [InlineData("ipad", SearchMode.Any, "ipad", true)]
        [InlineData("i-pad", SearchMode.All, "ipad", true)]
        [InlineData("i-pad", SearchMode.Any, "ipad", true)]
        [InlineData("ipad", SearchMode.All, "i pad", true)]
        [InlineData("ipad", SearchMode.Any, "i pad", true)]
        [InlineData("i pad", SearchMode.All, "i-pad", true)]
        [InlineData("i pad", SearchMode.Any, "i-pad", true)]
        [InlineData("xxx i pad", SearchMode.All, "ipad", false)]
        [InlineData("xxx i pad", SearchMode.Any, "ipad", true)]
        [InlineData("xxx i-pad", SearchMode.All, "ipad", false)]
        [InlineData("xxx i-pad", SearchMode.Any, "ipad", true)]
        [InlineData("xxx ipad", SearchMode.All, "ipad", false)]
        [InlineData("xxx ipad", SearchMode.Any, "ipad", true)]
        [InlineData("pad", SearchMode.All, "i pad", true)]
        [InlineData("pad", SearchMode.Any, "i pad", true)]
        [InlineData("pad", SearchMode.All, "ipad", true)]
        [InlineData("pad", SearchMode.Any, "ipad", true)]
        //// ExplicitMapping : i phone, i-phone => iphone
        [InlineData("i-phone", SearchMode.All, "", false)]
        [InlineData("i-phone", SearchMode.Any, "", false)]
        [InlineData("i-phone", SearchMode.All, "xxx", false)]
        [InlineData("i-phone", SearchMode.Any, "xxx", false)]
        [InlineData("i phone", SearchMode.All, "i phone", true)]
        [InlineData("i phone", SearchMode.Any, "i phone", true)]
        [InlineData("i-phone", SearchMode.All, "i phone", true)]
        [InlineData("i-phone", SearchMode.Any, "i-phone", true)]
        [InlineData("iphone", SearchMode.All, "iphone", true)]
        [InlineData("iphone", SearchMode.Any, "iphone", true)]
        [InlineData("i-phone", SearchMode.All, "iphone", true)]
        [InlineData("i-phone", SearchMode.Any, "iphone", true)]
        [InlineData("iphone", SearchMode.All, "i phone", true)]
        [InlineData("iphone", SearchMode.Any, "i phone", true)]
        [InlineData("i phone", SearchMode.All, "i-phone", true)]
        [InlineData("i phone", SearchMode.Any, "i-phone", true)]
        [InlineData("xxx i phone", SearchMode.All, "iphone", false)]
        [InlineData("xxx i phone", SearchMode.Any, "iphone", true)]
        [InlineData("xxx i-phone", SearchMode.All, "iphone", false)]
        [InlineData("xxx i-phone", SearchMode.Any, "iphone", true)]
        [InlineData("xxx iphone", SearchMode.All, "iphone", false)]
        [InlineData("xxx iphone", SearchMode.Any, "iphone", true)]
        [InlineData("phone", SearchMode.All, "i phone", false)]
        [InlineData("phone", SearchMode.Any, "i phone", false)]
        [InlineData("phone", SearchMode.All, "iphone", false)]
        [InlineData("phone", SearchMode.Any, "iphone", false)]
        //// ExplicitMapping : i pod, i-pod, ipod => i pod, i-pod, ipod
        [InlineData("i-pod", SearchMode.All, "", false)]
        [InlineData("i-pod", SearchMode.Any, "", false)]
        [InlineData("i-pod", SearchMode.All, "xxx", false)]
        [InlineData("i-pod", SearchMode.Any, "xxx", false)]
        [InlineData("i pod", SearchMode.All, "i pod", true)]
        [InlineData("i pod", SearchMode.Any, "i pod", true)]
        [InlineData("i-pod", SearchMode.All, "i pod", true)]
        [InlineData("i-pod", SearchMode.Any, "i-pod", true)]
        [InlineData("ipod", SearchMode.All, "ipod", true)]
        [InlineData("ipod", SearchMode.Any, "ipod", true)]
        [InlineData("i-pod", SearchMode.All, "ipod", true)]
        [InlineData("i-pod", SearchMode.Any, "ipod", true)]
        [InlineData("ipod", SearchMode.All, "i pod", true)]
        [InlineData("ipod", SearchMode.Any, "i pod", true)]
        [InlineData("i pod", SearchMode.All, "i-pod", true)]
        [InlineData("i pod", SearchMode.Any, "i-pod", true)]
        [InlineData("xxx i pod", SearchMode.All, "ipod", false)]
        [InlineData("xxx i pod", SearchMode.Any, "ipod", true)]
        [InlineData("xxx i-pod", SearchMode.All, "ipod", false)]
        [InlineData("xxx i-pod", SearchMode.Any, "ipod", true)]
        [InlineData("xxx ipod", SearchMode.All, "ipod", false)]
        [InlineData("xxx ipod", SearchMode.Any, "ipod", true)]
        [InlineData("pod", SearchMode.All, "i pod", true)]
        [InlineData("pod", SearchMode.Any, "i pod", true)]
        [InlineData("pod", SearchMode.All, "ipod", true)]
        [InlineData("pod", SearchMode.Any, "ipod", true)]
        //// ExplicitMapping : imac => i mac, i-mac, imac
        [InlineData("i-mac", SearchMode.All, "", false)]
        [InlineData("i-mac", SearchMode.Any, "", false)]
        [InlineData("i-mac", SearchMode.All, "xxx", false)]
        [InlineData("i-mac", SearchMode.Any, "xxx", false)]
        [InlineData("i mac", SearchMode.All, "i mac", true)]
        [InlineData("i mac", SearchMode.Any, "i mac", true)]
        [InlineData("i-mac", SearchMode.All, "i mac", true)]
        [InlineData("i-mac", SearchMode.Any, "i-mac", true)]
        [InlineData("imac", SearchMode.All, "imac", true)]
        [InlineData("imac", SearchMode.Any, "imac", true)]
        [InlineData("i-mac", SearchMode.All, "imac", true)]
        [InlineData("i-mac", SearchMode.Any, "imac", true)]
        [InlineData("imac", SearchMode.All, "i mac", false)]
        [InlineData("imac", SearchMode.Any, "i mac", false)]
        [InlineData("i mac", SearchMode.All, "i-mac", true)]
        [InlineData("i mac", SearchMode.Any, "i-mac", true)]
        [InlineData("xxx i mac", SearchMode.All, "imac", false)]
        [InlineData("xxx i mac", SearchMode.Any, "imac", true)]
        [InlineData("xxx i-mac", SearchMode.All, "imac", false)]
        [InlineData("xxx i-mac", SearchMode.Any, "imac", true)]
        [InlineData("xxx imac", SearchMode.All, "imac", false)]
        [InlineData("xxx imac", SearchMode.Any, "imac", true)]
        [InlineData("mac", SearchMode.All, "i mac", true)]
        [InlineData("mac", SearchMode.Any, "i mac", true)]
        [InlineData("mac", SearchMode.All, "imac", true)]
        [InlineData("mac", SearchMode.Any, "imac", true)]
        public void StandardMatches(string search, SearchMode mode, string document, bool expected)
        {
            var synonymMap =
                new SynonymMap(
                    new StandardAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                        new ExplicitMappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
                        new ExplicitMappingSynonym(new[] { "i pod", "i-pod", "ipod" }, new[] { "i pod", "i-pod", "ipod" }),
                        new ExplicitMappingSynonym(new[] { "imac" }, new[] { "i mac", "i-mac", "imac" }),
                    });

            Assert.Equal(expected, IsMatching(synonymMap, search, mode, document));
        }

        [Fact]
        public void BacktrackingPosition()
        {
            var synonymMap =
                new SynonymMap(
                    new StandardAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "aaa", "xxx", "yyy", "zzz" }),
                        new ExplicitMappingSynonym(new[] { "bbb" }, new[] { "ccc" }),
                        new EquivalencySynonym(new[] { "mmm nnn ooo", "ppp" }),
                    });

            var phrase = "aaa a a bbb a a mmm nnn a a mmm nnn";

            var input = synonymMap.Analyzer.CreateTokenTermSource().GetTerms(phrase).Select(x => new { Token = x.Token.ToString(), x.Position }).ToList();
            var indexOutput = synonymMap.CreateIndexTokenTermSource().GetTerms(phrase).Select(x => new { Token = x.Token.ToString(), x.Position }).ToList();
            var searchOutput = synonymMap.CreateSearchTokenTermSource().GetTerms(phrase).Select(x => new { Token = x.Token.ToString(), x.Position }).ToList();

            var expected = string.Join(", ", input.Select(x => x.Position).Distinct());
            var indexActual = string.Join(", ", indexOutput.Select(x => x.Position).Distinct());
            var searchActual = string.Join(", ", searchOutput.Select(x => x.Position).Distinct());

            Assert.Equal(expected, indexActual);
            Assert.Equal(expected, searchActual);
        }

        [Fact]
        public void TextParsing()
        {
            var synonyms =
                new Synonym[]
                {
                    new EquivalencySynonym(new[] { "i pad", "i-pad", "ipad" }),
                    new ExplicitMappingSynonym(new[] { "i phone", "i-phone" }, new[] { "iphone" }),
                    new ExplicitMappingSynonym(new[] { "i pod", "i-pod", "ipod" }, new[] { "i pod", "i-pod", "ipod" }),
                    new ExplicitMappingSynonym(new[] { "imac" }, new[] { "i mac", "i-mac", "imac" }),
                };

            var synonymsText = string.Join(Environment.NewLine, synonyms.Select(x => x.ToString()));

            Assert.Equal(
                """
                i pad, i-pad, ipad
                i phone, i-phone => iphone
                i pod, i-pod, ipod => i pod, i-pod, ipod
                imac => i mac, i-mac, imac
                """,
                synonymsText);

            var parsed = Synonym.Parse(synonymsText);

            Assert.True(synonyms.Select(x => x.ToString()).SequenceEqual(parsed.Select(x => x.ToString())));
        }

        private static bool IsMatching(ISynonymMap synonymMap, string search, SearchMode mode, string document)
        {
            var mapper = new SynonymMapper(synonymMap, document);
            var index = IndexBuilder.Build(mapper.Documents, mapper);

            using var result = index.Query(
                index.QueryBuilder()
                    .Search(mapper.Field, mode, search));

            return result.Documents.Count == 1;
        }

        private sealed class SynonymMapper : IIndexMapper<SynonymMapper.SynonymDocument>
        {
            private readonly TextField<SynonymDocument> field;
            private readonly SynonymDocument document;

            public SynonymMapper(ISynonymMap synonymMap, string document)
            {
                this.field = new TextField<SynonymDocument>(x => x.Text, synonymMap.Analyzer, synonymMap);
                this.document = new SynonymDocument(document);
            }

            public TextField<SynonymDocument> Field
            {
                get
                {
                    return this.field;
                }
            }

            public IEnumerable<SynonymDocument> Documents
            {
                get
                {
                    yield return this.document;
                }
            }

            public SynonymDocument GetDocument(SynonymDocument item)
            {
                return item;
            }

            public string GetDocumentKey(SynonymDocument item)
            {
                return item.Key;
            }

            public IEnumerable<Field> GetFields()
            {
                yield return this.field;
            }

            public sealed class SynonymDocument
            {
                public SynonymDocument(string text)
                {
                    this.Text = text;
                }

                public string Key { get; } = Guid.NewGuid().ToString();

                public string Text { get; }
            }
        }
    }
}
