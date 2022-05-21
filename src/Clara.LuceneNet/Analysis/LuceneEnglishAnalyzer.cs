using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis.En;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishAnalyzer : ITokenizer
    {
        private static readonly DisposableThreadLocal<EnglishAnalyzer> Analyzer = new(() => new EnglishAnalyzer(LuceneVersion.LUCENE_48));

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

            var analyzer = Analyzer.Value;

            using (var input = new StringReader(text))
            {
                using (var tokenStream = analyzer.GetTokenStream(string.Empty, input))
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
