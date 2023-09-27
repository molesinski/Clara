using System.Collections;
using Clara.Utils;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public class LuceneEnglishPorterStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<OperationContext> Pool = new(() => new());

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var context = Pool.Lease();

            context.Instance.SetToken(token);

            foreach (var stem in context.Instance)
            {
                return stem;
            }

            return default;
        }

        private sealed class OperationContext : IEnumerable<Token>, IDisposable
        {
            private readonly SingleTokenStream stringStream;
            private readonly PorterStemFilter stemmer;

            public OperationContext()
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
