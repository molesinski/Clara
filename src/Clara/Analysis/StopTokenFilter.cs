using Clara.Utils;

namespace Clara.Analysis
{
    public class StopTokenFilter : ITokenFilter
    {
        private readonly HashSetSlim<Token> stopwords = new();

        public StopTokenFilter(IEnumerable<string> stopwords)
        {
            if (stopwords is null)
            {
                throw new ArgumentNullException(nameof(stopwords));
            }

            foreach (var stopword in stopwords)
            {
                this.stopwords.Add(new Token(stopword));
            }
        }

        public IEnumerable<Token> Stopwords
        {
            get
            {
                return this.stopwords;
            }
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (this.stopwords.Contains(token))
            {
                return default;
            }

            return next(token);
        }
    }
}
