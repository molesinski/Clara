using System.Collections;
using System.Collections.Generic;
using Clara.Analysis.Stemming;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public class EnglishPorterStemmer : IStemmer
    {
        private static readonly ObjectPool<Stemmer> StemmerPool = new(() => new());

        public Token Stem(Token token)
        {
            var stemmer = StemmerPool.Get();

            try
            {
                stemmer.SetToken(token);

                foreach (var stem in stemmer)
                {
                    return stem;
                }

                return default;
            }
            finally
            {
                StemmerPool.Return(stemmer);
            }
        }

        private class Stemmer : IEnumerable<Token>
        {
            private readonly SingleTokenStream stringStream;
            private readonly PorterStemFilter stemmer;

            public Stemmer()
            {
                this.stringStream = new SingleTokenStream();
                this.stemmer = new PorterStemFilter(this.stringStream);
            }

            public void SetToken(Token token)
            {
                this.stringStream.SetToken(token);
            }

            public TokenStreamEnumerable.Enumerator GetEnumerator()
            {
                return new TokenStreamEnumerable(this.stemmer).GetEnumerator();
            }

            IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
            {
                return new TokenStreamEnumerable(this.stemmer).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new TokenStreamEnumerable(this.stemmer).GetEnumerator();
            }
        }
    }
}
