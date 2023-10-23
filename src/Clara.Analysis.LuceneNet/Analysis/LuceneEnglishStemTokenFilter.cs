using Clara.Utils;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.En;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<OperationContext> Pool = new(() => new());

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var context = Pool.Lease();

            foreach (var stem in new TokenStreamEnumerable(context.Instance.GetTokenStream(token)))
            {
                if (!stem.IsEmpty)
                {
                    token.Set(stem.AsReadOnlySpan());
                }

                break;
            }

            return token;
        }

        private sealed class OperationContext : IDisposable
        {
            private readonly SingleTokenStream tokenTermSource;
            private readonly PorterStemFilter stemmer;

            public OperationContext()
            {
                this.tokenTermSource = new SingleTokenStream();
                this.stemmer = new PorterStemFilter(this.tokenTermSource);
            }

            public TokenStream GetTokenStream(Token token)
            {
                this.tokenTermSource.SetToken(token);

                return this.stemmer;
            }

            public void Dispose()
            {
                this.stemmer.Dispose();
                this.tokenTermSource.Dispose();
            }
        }
    }
}
