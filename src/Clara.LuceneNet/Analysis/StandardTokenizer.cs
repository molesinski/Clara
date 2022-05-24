using System.Collections.Generic;
using System.IO;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public class StandardTokenizer : ITokenizer
    {
        private static readonly ObjectPool<Lucene.Net.Analysis.Standard.StandardTokenizer> TokenizerPool = new(() => new(LuceneVersion.LUCENE_48, new StringReader(string.Empty)));

        public IEnumerable<string> GetTokens(string text)
        {
            var tokenizer = TokenizerPool.Get();

            try
            {
                using (var reader = new StringReader(text))
                {
                    using (tokenizer)
                    {
                        tokenizer.SetReader(reader);

                        foreach (var token in new TokenStreamEnumerable(tokenizer))
                        {
                            yield return token;
                        }
                    }
                }
            }
            finally
            {
                TokenizerPool.Return(tokenizer);
            }
        }
    }
}
