using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardTokenizer : ITokenizer
    {
        public IEnumerable<string> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                yield break;
            }

            using (var input = new StringReader(text))
            {
                using (var tokenStream = new StandardTokenizer(LuceneVersion.LUCENE_48, input))
                {
                    foreach (var token in new TokenStreamEnumerable(tokenStream))
                    {
                        yield return token;
                    }
                }
            }
        }
    }
}
