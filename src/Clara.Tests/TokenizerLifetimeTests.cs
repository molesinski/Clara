#pragma warning disable CA1861 // Avoid constant arrays as arguments

using System.Runtime.CompilerServices;
using Clara.Analysis;
using Clara.Analysis.Synonyms;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Xunit;

namespace Clara.Tests
{
    public class TokenizerLifetimeTests
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void DisposedEnumerationReleasesTextAndAllowsReuse(bool whitespace, bool stopEarly)
        {
            var source = CreateTokenizer(whitespace).CreateTokenTermSource();
            var reference = EnumerateText(source, stopEarly);

            AssertCollected(reference, source);

            var terms = source.GetTerms("red blue").Select(x => (x.Token.ToString(), x.Position)).ToArray();

            Assert.Equal(new[] { ("red", new Position(0)), ("blue", new Position(1)) }, terms);
        }

        [Theory]
        [InlineData(false, false, SearchMode.Any)]
        [InlineData(false, false, SearchMode.All)]
        [InlineData(false, true, SearchMode.Any)]
        [InlineData(false, true, SearchMode.All)]
        [InlineData(true, false, SearchMode.Any)]
        [InlineData(true, false, SearchMode.All)]
        [InlineData(true, true, SearchMode.Any)]
        [InlineData(true, true, SearchMode.All)]
        public void PooledQuerySourceReleasesTextWhileIndexRemainsAlive(bool whitespace, bool synonyms, SearchMode mode)
        {
            var mapper = new TextMapper(CreateTokenizer(whitespace), synonyms);
            var index = IndexBuilder.Build(new[] { "red blue" }, mapper, new InstanceTokenEncoderStore());
            var reference = QueryText(index, mapper.Text, mode);

            AssertCollected(reference, index);

            using var result = index.Query(q => q.Search(mapper.Text, SearchMode.All, "red"));

            Assert.Single(result.Documents);
        }

        private static ITokenizer CreateTokenizer(bool whitespace)
        {
            return whitespace ? new WhitespaceTokenizer() : new StandardTokenizer();
        }

        // Keep the generated string out of the collecting method's stack, even with JIT inlining.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference EnumerateText(ITokenTermSource source, bool stopEarly)
        {
            var text = "missing " + new string(' ', 65536) + "red";
            var reference = new WeakReference(text);
            var count = 0;

            foreach (var term in source.GetTerms(text))
            {
                count++;

                if (stopEarly)
                {
                    break;
                }
            }

            Assert.Equal(stopEarly ? 1 : 2, count);

            return reference;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static WeakReference QueryText(Index<string> index, TextField<string> field, SearchMode mode)
        {
            var text = "missing " + new string(' ', 65536) + "red";
            var reference = new WeakReference(text);
            using var result = index.Query(q => q.Search(field, mode, text));

            Assert.Equal(mode == SearchMode.All ? 0 : 1, result.Documents.Count);

            return reference;
        }

        private static void AssertCollected(WeakReference reference, object owner)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Assert.False(reference.IsAlive);
            GC.KeepAlive(owner);
        }

        private sealed class TextMapper : IIndexMapper<string>
        {
            public TextMapper(ITokenizer tokenizer, bool synonyms)
            {
                var analyzer = new Analyzer(tokenizer);
                var map = synonyms ? new SynonymMap(analyzer, Synonym.Parse("scarlet => red")) : null;

                this.Text = new TextField<string>(x => x, analyzer, map);
            }

            public TextField<string> Text { get; }

            public IEnumerable<Field> GetFields()
            {
                yield return this.Text;
            }

            public string GetDocumentKey(string item)
            {
                return item;
            }

            public string GetDocument(string item)
            {
                return item;
            }
        }
    }
}
