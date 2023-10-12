using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public abstract class SnowballAnalyzer<TStemFilter, TStopFilter, TStemmer> : IAnalyzer
        where TStemFilter : SnowballStemTokenFilter<TStemmer>, new()
        where TStopFilter : StopTokenFilter, new()
        where TStemmer : Stemmer, new()
    {
        private readonly IAnalyzer analyzer;

        protected SnowballAnalyzer(
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
                filters.Add(new TStopFilter());
            }

            if (keywords is not null)
            {
                filters.Add(new KeywordTokenFilter(keywords));
            }

            filters.Add(new TStemFilter());

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
