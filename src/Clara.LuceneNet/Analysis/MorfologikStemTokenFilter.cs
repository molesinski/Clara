using System;
using System.Collections.Generic;
using Lucene.Net.Util;
using Morfologik.Stemming;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public class MorfologikStemTokenFilter : ITokenFilter
    {
        private static readonly DisposableThreadLocal<IStemmer> Stemmer = new(() => new PolishStemmer());

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var stemmer = Stemmer.Value!;

            foreach (var token in tokens)
            {
                var hadStems = false;
                var lemmas = stemmer.Lookup(token);

                foreach (var lemma in lemmas)
                {
                    var stem = lemma.GetStem();

                    if (stem is not null)
                    {
                        hadStems = true;

                        yield return stem.ToString();
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
