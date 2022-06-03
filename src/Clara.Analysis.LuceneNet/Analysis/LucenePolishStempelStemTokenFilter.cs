using System.Text;
using Egothor.Stemmer;
using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStempelStemTokenFilter : ITokenFilter
    {
        private readonly ObjectPool<StringBuilder> pool;
        private readonly Trie stemmer;
        private readonly bool tokenOnEmptyStem;

        public LucenePolishStempelStemTokenFilter(bool tokenOnEmptyStem = true)
        {
            this.pool = new(() => new(capacity: Token.MaximumLength));
            this.stemmer = PolishAnalyzer.DefaultTable;
            this.tokenOnEmptyStem = tokenOnEmptyStem;
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            var builder = this.pool.Get();

            try
            {
                var tokenString = token.ToString();
                var result = this.stemmer.GetLastOnPath(tokenString);

                if (result is not null)
                {
                    builder.Clear();
                    builder.Append(tokenString);

                    Diff.Apply(builder, result);

                    if (builder.Length > 0)
                    {
                        var stem = StringHelper.Create(builder);

                        return new Token(stem);
                    }
                }

                if (this.tokenOnEmptyStem)
                {
                    return token;
                }

                return default;
            }
            finally
            {
                this.pool.Return(builder);
            }
        }
    }
}
