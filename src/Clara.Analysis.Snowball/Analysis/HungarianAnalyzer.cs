using Clara.Utils;

namespace Clara.Analysis
{
    public sealed class HungarianAnalyzer : IAnalyzer
    {
        private readonly IAnalyzer analyzer;

        public HungarianAnalyzer(
            IEnumerable<string>? stopwords = null,
            IEnumerable<string>? keywords = null)
        {
            var filters = new ListSlim<ITokenFilter>();

            filters.Add(new LowerInvariantTokenFilter());

            if (stopwords is not null)
            {
                filters.Add(new StopTokenFilter(stopwords));
            }
            else
            {
                filters.Add(new HungarianStopTokenFilter());
            }

            if (keywords is not null)
            {
                filters.Add(new KeywordTokenFilter(keywords));
            }

            filters.Add(new HungarianStemTokenFilter());

            this.analyzer = new Analyzer(new BasicTokenizer(), filters);
        }

        public ITokenizer Tokenizer
        {
            get
            {
                return this.analyzer.Tokenizer;
            }
        }

        public ITokenTermSource CreateTokenTermSource()
        {
            return this.analyzer.CreateTokenTermSource();
        }
    }
}
