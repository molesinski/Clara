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
            using var stemmer = this.pool.Lease();

            stemmer.Instance.SetToken(token);

            foreach (var stem in stemmer.Instance)
            {
                return stem;
            }

            return default;
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

            public ReadOnlyTokenStreamEnumerable.Enumerator GetEnumerator()
            {
                return new ReadOnlyTokenStreamEnumerable(this.stemmer).GetEnumerator();
            }

            IEnumerator<Token> IEnumerable<Token>.GetEnumerator()
            {
                return new ReadOnlyTokenStreamEnumerable(this.stemmer).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ReadOnlyTokenStreamEnumerable(this.stemmer).GetEnumerator();
            }

            public void Dispose()
            {
                this.stemmer.Dispose();
                this.stringStream.Dispose();
            }
        }
    }
}
