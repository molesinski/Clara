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
        public void EmptyTokenizer()
        {
            foreach (var token in this.tokenizer.GetTokens(string.Empty))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void EmptyAnalyzer()
        {
            foreach (var token in this.analyzer.GetTokens(string.Empty))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void EmptySynonymMap()
        {
            foreach (var token in this.synonymMap.GetTokens(string.Empty))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void PhraseTokenizer()
        {
            foreach (var token in this.tokenizer.GetTokens("The quick brown fox jumps over the lazy dog"))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void PhraseAnalyzer()
        {
            foreach (var token in this.analyzer.GetTokens("The quick brown fox jumps over the lazy dog"))
            {
                _ = token;
            }
        }

        [Benchmark]
        public void PhraseSynonymMap()
        {
            foreach (var token in this.synonymMap.GetTokens("The quick brown fox jumps over the lazy dog"))
            {
                _ = token;
            }
        }
    }
}
