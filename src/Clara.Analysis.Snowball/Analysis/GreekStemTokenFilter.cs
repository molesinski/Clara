using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public sealed class GreekStemTokenFilter : ITokenFilter
    {
        private readonly ObjectPool<GreekStemmer> pool;

        public GreekStemTokenFilter()
        {
            this.pool = new(() => new());
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var stemmer = this.pool.Lease();

            var stem = stemmer.Instance.Stem(token.ToString());

            if (stem.Length > 0)
            {
                return new Token(stem);
            }

            return default;
        }
    }
}
