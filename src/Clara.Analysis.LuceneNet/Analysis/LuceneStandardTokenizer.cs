using System.Collections.Generic;
using System.IO;
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
            var tokenizer = this.pool.Get();

            try
            {
                using var reader = new StringReader(text);
                using var disposable = tokenizer;

                tokenizer.SetReader(reader);

                foreach (var token in new TokenStreamEnumerable(tokenizer))
                {
                    yield return token;
                }
            }
            finally
            {
                this.pool.Return(tokenizer);
            }
        }
    }
}
