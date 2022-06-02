using System.Text;
using Clara.Analysis.Stemming;
using Egothor.Stemmer;

namespace Clara.Analysis
{
    public sealed class PolishStempelStemmer : IStemmer
    {
        private static readonly ObjectPool<StringBuilder> BuilderPool = new(() => new StringBuilder());

        private readonly Trie stemmer = Lucene.Net.Analysis.Pl.PolishAnalyzer.DefaultTable;

        public Token Stem(Token token)
        {
            var builder = BuilderPool.Get();

            try
            {
                var result = this.stemmer.GetLastOnPath(token.ToString());

                if (result is not null)
                {
                    builder.Clear();
                    builder.Append(token);

                    Diff.Apply(builder, result);

                    if (builder.Length > 0)
                    {
                        var stem = StringHelper.Create(builder);

                        return new Token(stem);
                    }
                }

                return default;
            }
            finally
            {
                BuilderPool.Return(builder);
            }
        }
    }
}
