using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.En;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishAnalyzer : ITokenizer, IDisposable
    {
        private readonly EnglishAnalyzer analyzer;

        public LuceneEnglishAnalyzer()
        {
            this.analyzer = new EnglishAnalyzer(LuceneVersion.LUCENE_48);
        }

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

            using (var tokenStream = this.analyzer.GetTokenStream(string.Empty, text))
            {
                foreach (var token in new TokenStreamEnumerable(tokenStream))
                {
                    yield return token;
                }
            }
        }

        public void Dispose()
        {
            this.analyzer.Dispose();
        }
    }
}
