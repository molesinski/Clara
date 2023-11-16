#pragma warning disable CA1861 // Avoid constant arrays as arguments

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

        private readonly ITokenTermSource standardTokenizer;
        private readonly ITokenTermSource standardAnalyzer;
        private readonly ITokenTermSource porterAnalyzer;
        private readonly ITokenTermSource synonymMap;
        private readonly ITokenTermSource englishAnalyzer;
        private readonly ITokenTermSource morfologikAnalyzer;

        public TokenizationBenchmarks()
        {
            this.standardTokenizer = new StandardTokenizer().CreateTokenTermSource();
            this.standardAnalyzer = new StandardAnalyzer().CreateTokenTermSource();
            this.porterAnalyzer = new PorterAnalyzer().CreateTokenTermSource();
            this.englishAnalyzer = new EnglishAnalyzer().CreateTokenTermSource();
            this.morfologikAnalyzer = new MorfologikAnalyzer().CreateTokenTermSource();

            var synonymMap =
                new SynonymMap(
                    new PorterAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "dog", "fox" }),
                    });

            this.synonymMap = synonymMap.CreateIndexTokenTermSource();
        }

        [Benchmark]
        public void StandardTokenizer()
        {
            foreach (var term in this.standardTokenizer.GetTerms(EnglishPhrase))
            {
                _ = term;
            }
        }

        [Benchmark]
        public void StandardAnalyzer()
        {
            foreach (var term in this.standardAnalyzer.GetTerms(EnglishPhrase))
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
        public void MorfologikAnalyzer()
        {
            foreach (var term in this.morfologikAnalyzer.GetTerms(PolishPhrase))
            {
                _ = term;
            }
        }
    }
}
