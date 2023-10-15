using Clara.Utils;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LucenePorterStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<OperationContext> Pool = new(() => new());

        public void Process(ref Token token, TokenFilterDelegate next)
        {
            using var context = Pool.Lease();

            foreach (var stem in new ReadOnlyTokenStreamEnumerable(context.Instance.GetTokenStream(token)))
            {
                if (!stem.IsEmpty)
                {
                    token.Set(stem.AsReadOnlySpan());
                }

                break;
            }
        }

        private sealed class OperationContext : IDisposable
        {
            private readonly SingleTokenStream tokenStream;
            private readonly PorterStemFilter stemmer;

            public OperationContext()
            {
                this.tokenStream = new SingleTokenStream();
                this.stemmer = new PorterStemFilter(this.tokenStream);
            }

            public TokenStream GetTokenStream(Token token)
            {
                this.tokenStream.SetToken(token);

                return this.stemmer;
            }

            public void Dispose()
            {
                this.stemmer.Dispose();
                this.tokenStream.Dispose();
            }
        }
    }
}
