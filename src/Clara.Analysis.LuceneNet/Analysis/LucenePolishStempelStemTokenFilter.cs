using System.Text;
using Clara.Utils;
using Egothor.Stemmer;
using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStempelStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<StringBuilder> Pool = new(() => new(capacity: Token.MaximumLength), builder => builder.Clear());

        private readonly Trie stemmer;
        private readonly bool tokenOnEmptyStem;

        public LucenePolishStempelStemTokenFilter(bool tokenOnEmptyStem = true)
        {
            this.stemmer = PolishAnalyzer.DefaultTable;
            this.tokenOnEmptyStem = tokenOnEmptyStem;
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var builder = Pool.Lease();

            var tokenString = token.ToString();
            var result = this.stemmer.GetLastOnPath(tokenString);

            if (result is not null)
            {
                builder.Instance.Append(tokenString);

                Diff.Apply(builder.Instance, result);

                if (builder.Instance.Length > 0)
                {
                    return new Token(builder.Instance.ToString());
                }
            }

            if (this.tokenOnEmptyStem)
            {
                return token;
            }

            return default;
        }
    }
}
