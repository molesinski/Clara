using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public sealed class SpanishStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<SpanishStemmer> Pool = new(() => new());

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var stemmer = Pool.Lease();

            var stem = stemmer.Instance.Stem(token.ToString());

            if (stem.Length > 0)
            {
                return new Token(stem);
            }

            return default;
        }
    }
}
