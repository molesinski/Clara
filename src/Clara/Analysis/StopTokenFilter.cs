using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public class StopTokenFilter : ITokenFilter
    {
        private readonly HashSet<string> stopwords;

        public StopTokenFilter(IEnumerable<string> stopwords)
            : this(stopwords, StringComparer.OrdinalIgnoreCase)
        {
        }

        public StopTokenFilter(IEnumerable<string> stopwords, IEqualityComparer<string> comparer)
        {
            if (stopwords is null)
            {
                throw new ArgumentNullException(nameof(stopwords));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.stopwords = new HashSet<string>(stopwords, comparer);
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
