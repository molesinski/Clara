using BenchmarkDotNet.Attributes;
using Clara.Analysis;
using Clara.Analysis.Synonyms;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class TokenizationBenchmarks
    {
        private readonly ITokenizer tokenizer;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap synonymMap;

        public TokenizationBenchmarks()
        {
            this.tokenizer = new BasicTokenizer();

            this.analyzer = new PorterAnalyzer();

            this.synonymMap =
                new SynonymMap(
                    new PorterAnalyzer(),
                    new Synonym[]
                    {
                        new EquivalencySynonym(new[] { "dog", "fox" }),
                    });
        }

        [Benchmark]
        public void Tokenizer()
        {
            using var tokens = this.tokenizer.GetTokens("The quick brown fox jumps over the lazy dog");

            foreach (var token in tokens)
            {
                _ = token;
            }
        }

        [Benchmark]
        public void Analyzer()
        {
            using var tokens = this.analyzer.GetTokens("The quick brown fox jumps over the lazy dog");

            foreach (var token in tokens)
            {
                _ = token;
            }
        }

        [Benchmark]
        public void SynonymMap()
        {
            using var tokens = this.synonymMap.GetTokens("The quick brown fox jumps over the lazy dog");

            foreach (var token in tokens)
            {
                _ = token;
            }
        }
    }
}
