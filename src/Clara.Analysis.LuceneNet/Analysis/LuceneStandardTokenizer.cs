using Clara.Utils;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public class LuceneStandardTokenizer : ITokenizer
    {
        private readonly ObjectPool<StandardTokenizer> pool;

        public LuceneStandardTokenizer()
        {
            this.pool = new(() => new(LuceneVersion.LUCENE_48, new StringReader(string.Empty)));
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            using var tokenizer = this.pool.Lease();
            using var reader = new StringReader(text);
            using var disposable = tokenizer;

            tokenizer.Instance.SetReader(reader);

            foreach (var token in new TokenStreamEnumerable(tokenizer.Instance))
            {
                yield return token;
            }
        }
    }
}
