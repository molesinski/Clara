using Clara.Analysis;
using Xunit;

namespace Clara.Tests
{
    [Collection(nameof(SnowballTests))]
    public class SnowballTests
    {
        [Theory]
        [InlineData(typeof(DanishStemTokenFilter), "cd'erne", "cd")]
        [InlineData(typeof(EstonianStemTokenFilter), "alice'iga", "alice")]
        [InlineData(typeof(FinnishStemTokenFilter), "abachan", "abach")]
        [InlineData(typeof(FinnishStemTokenFilter), "ajatollahin", "ajatollah")]
        [InlineData(typeof(GermanStemTokenFilter), "einstein'schen", "einstein")]
        [InlineData(typeof(ItalianStemTokenFilter), "all'acqua", "acqua")]
        [InlineData(typeof(LithuanianStemTokenFilter), "brokeriai", "broker")]
        [InlineData(typeof(NorwegianStemTokenFilter), "bot'en", "bot")]
        [InlineData(typeof(PolishStemTokenFilter), "billy'emu", "billy")]
        [InlineData(typeof(PolishStemTokenFilter), "college'ach", "college")]
        [InlineData(typeof(FrenchStemTokenFilter), "z'arbres", "arbre")]
        [InlineData(typeof(EnglishStemTokenFilter), "skis", "ski")]
        [InlineData(typeof(EnglishStemTokenFilter), "interfering", "interfer")]
        [InlineData(typeof(DutchStemTokenFilter), "paarden", "paar")]
        public void StemsWordsAfterReusingPooledBuffers(Type filterType, string word, string expected)
        {
            var filter = (ITokenFilter)Activator.CreateInstance(filterType)!;
            var token = new Token(new char[Token.MaximumLength], 0);
            var longWord = new string('x', Token.MaximumLength);

            for (var iteration = 0; iteration < 3; iteration++)
            {
                token.Set(longWord);
                Assert.InRange(filter.Process(token, UnexpectedNext).Length, 1, Token.MaximumLength);

                token.Set(string.Empty);
                Assert.True(filter.Process(token, UnexpectedNext).IsEmpty);

                token.Set(word);
                Assert.Equal(new Token(expected), filter.Process(token, UnexpectedNext));

                token.Set("a");
                filter.Process(token, UnexpectedNext);
                token.Set(word);
                Assert.Equal(new Token(expected), filter.Process(token, UnexpectedNext));
            }
        }

#if NET8_0_OR_GREATER
        [Theory]
        [Trait("Category", "Performance")]
        [InlineData(typeof(EnglishStemTokenFilter), "running", 0)]
        [InlineData(typeof(PolishStemTokenFilter), "domami", 64)]
        public void WarmedStemmingPreservesAllocationBudget(Type filterType, string word, int bytesPerToken)
        {
            var filter = (ITokenFilter)Activator.CreateInstance(filterType)!;
            var token = new Token(new char[Token.MaximumLength], 0);
            TokenFilterDelegate next = UnexpectedNext;

            for (var iteration = 0; iteration < 1000; iteration++)
            {
                token.Set(word);
                filter.Process(token, next);
            }

            var before = GC.GetAllocatedBytesForCurrentThread();

            for (var iteration = 0; iteration < 1000; iteration++)
            {
                token.Set(word);
                filter.Process(token, next);
            }

            var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            // The Polish among callback can allocate a delegate before tiered JIT optimization.
            Assert.InRange(allocated, 0, bytesPerToken * 1000L);
        }
#endif

        private static Token UnexpectedNext(Token token)
        {
            throw new InvalidOperationException("Snowball stemming terminates the filter chain.");
        }
    }

    [CollectionDefinition(nameof(SnowballTests), DisableParallelization = true)]
    public class SnowballTestGroup
    {
    }
}
