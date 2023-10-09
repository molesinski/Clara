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
            foreach (var token in this.basicTokenizer.GetTokens(Phrase))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void PorterAnalyzer()
        {
            foreach (var token in this.porterAnalyzer.GetTokens(Phrase))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void SynonymMap()
        {
            foreach (var token in this.synonymMap.GetTokens(Phrase))
            {
                _ = token;
            }
        }
    }
}
