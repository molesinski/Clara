using System.Text;
using Clara.Analysis.Stemming;
using Egothor.Stemmer;
using Lucene.Net.Analysis.Pl;

namespace Clara.Analysis
{
    public sealed class PolishStempelStemmer : IStemmer
    {
        private static readonly ObjectPool<StringBuilder> BuilderPool = new(() => new StringBuilder());

        private readonly Trie stemmer = Lucene.Net.Analysis.Pl.PolishAnalyzer.DefaultTable;

        public StemResult Stem(string token)
        {
            var builder = BuilderPool.Get();

            try
            {
                var result = this.stemmer.GetLastOnPath(token);

                if (result is not null)
                {
                    builder.Clear();
                    builder.Append(token);

                    Diff.Apply(builder, result);

                    if (builder.Length > 0)
                    {
                        var stem = StringHelper.Create(builder);

                        return new StemResult(stem);
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
