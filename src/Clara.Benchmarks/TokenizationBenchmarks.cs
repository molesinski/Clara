using BenchmarkDotNet.Attributes;
using Clara.Analysis;
using Clara.Analysis.Synonyms;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class TokenizationBenchmarks
    {
        private const string EnglishPhrase = "The quick brown fox jumps over the lazy dog";
        private const string PolishPhrase = "Szybki brązowy lis skacze nad leniwym psem";

        private readonly ITokenizer basicTokenizer;
        private readonly IAnalyzer porterAnalyzer;
        private readonly IAnalyzer englishAnalyzer;
        private readonly IAnalyzer polishAnalyzer;
        private readonly ISynonymMap synonymMap;

        public TokenizationBenchmarks()
        {
            this.basicTokenizer = new BasicTokenizer();
            this.porterAnalyzer = new PorterAnalyzer();
            this.englishAnalyzer = new EnglishAnalyzer();
            this.polishAnalyzer = new PolishAnalyzer();

            this.synonymMap =
                new SynonymMap(
                    new PorterAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "dog", "fox" }),
                    });
        }

        [Benchmark]
        public void BasicTokenizer()
        {
            foreach (var term in this.basicTokenizer.GetTokens(EnglishPhrase))
            {
                _ = term;
            }
        }

        [Benchmark]
        public void PorterAnalyzer()
        {
            foreach (var term in this.porterAnalyzer.GetTerms(EnglishPhrase))
            {
                _ = term;
            }
        }

        [Benchmark]
        public void SynonymMap()
        {
            foreach (var token in this.synonymMap.GetTerms(EnglishPhrase))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void EnglishAnalyzer()
        {
            foreach (var term in this.englishAnalyzer.GetTerms(EnglishPhrase))
            {
                _ = term;
            }
        }

        [Benchmark]
        public void PolishAnalyzer()
        {
            foreach (var term in this.polishAnalyzer.GetTerms(PolishPhrase))
            {
                _ = term;
            }
        }
    }
}
