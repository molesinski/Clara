using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public class StopTokenFilter : ITokenFilter
    {
        private readonly HashSet<string> stopwords;

        public StopTokenFilter(IEnumerable<string> stopwords)
        {
            if (stopwords is null)
            {
                throw new ArgumentNullException(nameof(stopwords));
            }

            this.stopwords = new HashSet<string>(stopwords);
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            foreach (var token in tokens)
            {
                if (!this.stopwords.Contains(token))
                {
                    yield return token;
                }
            }
        }
    }
}
