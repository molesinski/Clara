using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public class StopTokenFilter : ITokenFilter
    {
        private readonly HashSet<Token> stopwords = new();

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

        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (this.stopwords.Contains(token))
            {
                return default;
            }

            return next(token);
        }
    }
}
