using System;
using System.Collections.Generic;

namespace Clara.Analysis.Stemming
{
    public class StemTokenFilter : ITokenFilter
    {
        private readonly IStemmer stemmer;
        private readonly bool emitTokenOnEmptyStems;
        private readonly bool acceptOneStemOnly;

        public StemTokenFilter(IStemmer stemmer, bool emitTokenOnEmptyStems = true, bool acceptOneStemOnly = false)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            this.stemmer = stemmer;
            this.emitTokenOnEmptyStems = emitTokenOnEmptyStems;
            this.acceptOneStemOnly = acceptOneStemOnly;
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

                    if (this.acceptOneStemOnly)
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
