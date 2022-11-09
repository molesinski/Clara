using System.Collections;
using Clara.Utils;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public class LuceneEnglishPorterStemTokenFilter : ITokenFilter
    {
        private readonly ObjectPool<Stemmer> pool;

        public LuceneEnglishPorterStemTokenFilter()
        {
            this.pool = new(() => new());
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            var stemmer = this.pool.Get();

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
                this.pool.Return(stemmer);
            }
        }

        private sealed class Stemmer : IEnumerable<Token>, IDisposable
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

            public void Dispose()
            {
                this.stemmer.Dispose();
                this.stringStream.Dispose();
            }
        }
    }
}
