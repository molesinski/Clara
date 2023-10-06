#pragma warning disable IDE0059 // Unnecessary assignment of a value

using BenchmarkDotNet.Attributes;
using Clara.Analysis;
using Clara.Analysis.Synonyms;

namespace Clara.Benchmarks
{
    [MemoryDiagnoser]
    public class AnalyzerBenchmarks
    {
        private readonly ITokenizer tokenizer;
        private readonly IAnalyzer analyzer;
        private readonly ISynonymMap synonymMap;

        public AnalyzerBenchmarks()
        {
            this.tokenizer =
                new BasicTokenizer();

            this.analyzer =
                new Analyzer(
                    new BasicTokenizer(),
                    new LowerInvariantTokenFilter(),
                    new PorterPossessiveTokenFilter(),
                    new PorterStopTokenFilter(),
                    new CachingTokenFilter(),
                    new LengthKeywordTokenFilter(),
                    new DigitsKeywordTokenFilter(),
                    new PorterStemTokenFilter());

            this.synonymMap =
                new SynonymMap(
                    this.analyzer,
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
            }
        }

        [Benchmark]
        public void Analyzer()
        {
            using var tokens = this.analyzer.GetTokens("The quick brown fox jumps over the lazy dog");

            foreach (var token in tokens)
            {
            }
        }

        [Benchmark]
        public void SynonymMap()
        {
            using var tokens = this.synonymMap.GetTokens("The quick brown fox jumps over the lazy dog");

            foreach (var token in tokens)
            {
            }
        }
    }
}
