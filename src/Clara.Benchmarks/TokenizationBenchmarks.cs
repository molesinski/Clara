using BenchmarkDotNet.Attributes;
using Clara.Analysis;
using Clara.Analysis.Synonyms;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class TokenizationBenchmarks
    {
        private const string Phrase = "The quick brown fox jumps over the lazy dog";

        private readonly ITokenizer basicTokenizer;
        private readonly IAnalyzer porterAnalyzer;
        private readonly ISynonymMap synonymMap;

        public TokenizationBenchmarks()
        {
            this.basicTokenizer = new BasicTokenizer();
            this.porterAnalyzer = new PorterAnalyzer();
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
            foreach (var term in this.basicTokenizer.GetTokens(Phrase))
            {
                _ = term;
            }
        }

        [Benchmark]
        public void PorterAnalyzer()
        {
            foreach (var term in this.porterAnalyzer.GetTerms(Phrase))
            {
                _ = term;
            }
        }

        [Benchmark]
        public void SynonymMap()
        {
            foreach (var token in this.synonymMap.GetTerms(Phrase))
            {
                _ = token;
            }
        }
    }
}
