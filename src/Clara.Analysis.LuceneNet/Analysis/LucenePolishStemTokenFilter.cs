using Clara.Utils;
using Lucene.Net.Analysis.Pl;
using Lucene.Net.Analysis.Stempel;

namespace Clara.Analysis
{
    public sealed class LucenePolishStemTokenFilter : ITokenFilter
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
            private readonly StempelFilter stemmer;

            public OperationContext()
            {
                this.tokenTermSource = new SingleTokenStream();
                this.stemmer = new StempelFilter(this.tokenTermSource, new StempelStemmer(PolishAnalyzer.DefaultTable));
            }

            public StempelFilter GetTokenStream(Token token)
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
