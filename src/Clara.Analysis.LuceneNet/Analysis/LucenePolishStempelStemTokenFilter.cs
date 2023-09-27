using System.Text;
using Clara.Utils;
using Egothor.Stemmer;
using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class LucenePolishStempelStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<OperationContext> Pool = new(() => new());

        private readonly Trie stemmer;
        private readonly bool tokenOnEmptyStem;

        public LucenePolishStempelStemTokenFilter(bool tokenOnEmptyStem = true)
        {
            this.stemmer = PolishAnalyzer.DefaultTable;
            this.tokenOnEmptyStem = tokenOnEmptyStem;
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var context = Pool.Lease();

            var builder = context.Instance.Builder;
            var tokenString = token.ToString();
            var result = this.stemmer.GetLastOnPath(tokenString);

            if (result is not null)
            {
                builder.Append(tokenString);

                Diff.Apply(builder, result);

                if (builder.Length > 0)
                {
                    return new Token(builder.ToString());
                }
            }

            if (this.tokenOnEmptyStem)
            {
                return token;
            }

            return default;
        }

        private sealed class OperationContext : IResettable
        {
            public OperationContext()
            {
                this.Builder = new(capacity: Token.MaximumLength);
            }

            public StringBuilder Builder { get; }

            void IResettable.Reset()
            {
                this.Builder.Clear();
            }
        }
    }
}
