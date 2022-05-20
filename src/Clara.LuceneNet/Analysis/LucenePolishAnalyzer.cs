using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.Pl;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LucenePolishAnalyzer : ITokenizer, IDisposable
    {
        private readonly PolishAnalyzer analyzer;

        public LucenePolishAnalyzer()
        {
            this.analyzer = new PolishAnalyzer(LuceneVersion.LUCENE_48);
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
