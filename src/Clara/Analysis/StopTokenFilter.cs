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
                if (!string.IsNullOrWhiteSpace(stopword))
                {
                    this.stopwords.Add(new Token(stopword));
                }
            }
        }

        public IReadOnlyCollection<Token> Stopwords
        {
            get
            {
                return this.stopwords;
            }
        }

        public void Process(in Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (this.stopwords.Count > 0)
            {
                if (this.stopwords.Contains(token))
                {
                    token.Clear();
                    return;
                }
            }

            next(in token);
        }
    }
}
