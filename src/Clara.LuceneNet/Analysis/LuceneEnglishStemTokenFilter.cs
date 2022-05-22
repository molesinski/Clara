using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishStemTokenFilter : ITokenFilter
    {
        private readonly IStringFactory stringFactory;

        public LuceneEnglishStemTokenFilter()
            : this(DefaultStringFactory.Instance)
        {
        }

        public LuceneEnglishStemTokenFilter(IStringFactory stringFactory)
        {
            if (stringFactory is null)
            {
                throw new ArgumentNullException(nameof(stringFactory));
            }

            this.stringFactory = stringFactory;
        }

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
                    foreach (var token in new TokenStreamEnumerable(tokenStream, this.stringFactory))
                    {
                        yield return token;
                    }
                }
            }
        }
    }
}
