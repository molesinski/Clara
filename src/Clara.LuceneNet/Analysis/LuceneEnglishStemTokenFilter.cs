using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishStemTokenFilter : ITokenFilter
    {
        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            using (var input = new EnumerableTokenStream(tokens))
            {
                using (var tokenStream = new PorterStemFilter(input))
                {
                    foreach (var token in new TokenStreamEnumerable(tokenStream))
                    {
                        yield return token;
                    }
                }
            }
        }
    }
}
