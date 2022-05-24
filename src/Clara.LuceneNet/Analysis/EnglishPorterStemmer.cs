using System.Collections.Generic;
using Clara.Analysis.Stemming;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public class EnglishPorterStemmer : IStemmer
    {
        private static readonly ObjectPool<Stemmer> StemmerPool = new(() => new());

        public StemResult Stem(string token)
        {
            var stemmer = StemmerPool.Get();

            try
            {
                stemmer.SetString(token);

                var firstStem = default(string);
                var stems = default(List<string>);

                foreach (var stem in new TokenStreamEnumerable(stemmer.TokenStream))
                {
                    if (firstStem == null)
                    {
                        firstStem = stem;
                    }
                    else if (stems == null)
                    {
                        stems = new List<string>();
                        stems.Add(firstStem);
                        stems.Add(stem);
                    }
                    else
                    {
                        stems.Add(stem);
                    }
                }

                if (stems != null)
                {
                    return new StemResult(stems);
                }
                else if (firstStem != null)
                {
                    return new StemResult(firstStem);
                }
                else
                {
                    return default;
                }
            }
            finally
            {
                StemmerPool.Return(stemmer);
            }
        }

        private class Stemmer
        {
            private readonly StringTokenStream stringStream;
            private readonly PorterStemFilter stemmer;

            public Stemmer()
            {
                this.stringStream = new StringTokenStream();
                this.stemmer = new PorterStemFilter(this.stringStream);
            }

            public TokenStream TokenStream
            {
                get
                {
                    return this.stemmer;
                }
            }

            public void SetString(string value)
            {
                this.stringStream.SetString(value);
            }
        }
    }
}
