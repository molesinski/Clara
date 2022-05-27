using System;
using System.Collections.Generic;

namespace Clara.Analysis.Stemming
{
    public class StemTokenFilter : ITokenFilter
    {
        private readonly IStemmer stemmer;
        private readonly bool emitTokenOnEmptyStems;
        private readonly bool acceptFirstStemOnly;

        public StemTokenFilter(IStemmer stemmer, bool emitTokenOnEmptyStems = true, bool acceptFirstStemOnly = false)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            this.stemmer = stemmer;
            this.emitTokenOnEmptyStems = emitTokenOnEmptyStems;
            this.acceptFirstStemOnly = acceptFirstStemOnly;
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            foreach (var token in tokens)
            {
                var hadStems = false;

                foreach (var stem in this.stemmer.Stem(token))
                {
                    hadStems = true;

                    yield return stem;

                    if (this.acceptFirstStemOnly)
                    {
                        break;
                    }
                }

                if (!hadStems)
                {
                    if (this.emitTokenOnEmptyStems)
                    {
                        yield return token;
                    }
                }
            }
        }
    }
}
