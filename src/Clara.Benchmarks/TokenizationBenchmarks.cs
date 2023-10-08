using BenchmarkDotNet.Attributes;
using Clara.Analysis;
using Clara.Analysis.Synonyms;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser(false)]
    [ShortRunJob]
    public class TokenizationBenchmarks
    {
        private const string Phrase = "The quick brown fox jumps over the lazy dog";

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
        public void TokenizerEmpty()
        {
            foreach (var token in this.tokenizer.GetTokens(string.Empty))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void TokenizerPhrase()
        {
            foreach (var token in this.tokenizer.GetTokens(Phrase))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void AnalyzerEmpty()
        {
            foreach (var token in this.analyzer.GetTokens(string.Empty))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void AnalyzerPhrase()
        {
            foreach (var token in this.analyzer.GetTokens(Phrase))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void SynonymMapEmpty()
        {
            foreach (var token in this.synonymMap.GetTokens(string.Empty))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void SynonymMapPhrase()
        {
            foreach (var token in this.synonymMap.GetTokens(Phrase))
            {
                _ = token;
            }
        }
    }
}
