using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class BasicAnalyzer : IAnalyzer
    {
        private readonly IAnalyzer analyzer;

        public BasicAnalyzer(
            IEnumerable<string>? stopwords = null)
        {
            var filters = new ListSlim<ITokenFilter>();

            filters.Add(new LowerInvariantTokenFilter());

            if (stopwords is not null)
            {
                filters.Add(new StopTokenFilter(stopwords));
            }

            this.analyzer = new Analyzer(new BasicTokenizer(), filters);
        }

        public ITokenizer Tokenizer
        {
            get
            {
                return this.analyzer.Tokenizer;
            }
        }

        public IEnumerable<AnalyzerTerm> GetTerms(string text)
        {
            return this.analyzer.GetTerms(text);
        }
    }
}
