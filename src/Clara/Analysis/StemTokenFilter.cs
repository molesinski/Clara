using System;

namespace Clara.Analysis
{
    public class StemTokenFilter : ITokenFilter
    {
        private readonly IStemmer stemmer;

        public StemTokenFilter(IStemmer stemmer)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            this.stemmer = stemmer;
        }

        public Token Process(Token token)
        {
            return this.stemmer.Stem(token);
        }
    }
}
