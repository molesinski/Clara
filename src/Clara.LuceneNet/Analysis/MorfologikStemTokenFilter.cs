using System;
using System.Collections.Generic;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public class MorfologikStemTokenFilter : ITokenFilter
    {
        private readonly PolishStemmer stemmer;

        public MorfologikStemTokenFilter()
        {
            this.stemmer = new PolishStemmer();
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            foreach (var token in tokens)
            {
                var hadStems = false;
                var lemmas = this.stemmer.Lookup(token);

                foreach (var lemma in lemmas)
                {
                    var stem = lemma.GetStem()?.ToString();

                    if (stem is not null)
                    {
                        hadStems = true;
                        yield return stem;
                    }
                }

                if (!hadStems)
                {
                    yield return token;
                }
            }
        }
    }
}
