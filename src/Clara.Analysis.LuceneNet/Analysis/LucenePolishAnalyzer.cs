using System;
using System.Collections.Generic;
using System.IO;
using Clara.Utils;
using Lucene.Net.Analysis.Pl;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LucenePolishAnalyzer : IAnalyzer
    {
        private readonly ObjectPool<PolishAnalyzer> pool;

        public LucenePolishAnalyzer()
        {
            this.pool = new(() => new(LuceneVersion.LUCENE_48));
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

            var analyzer = this.pool.Get();

            try
            {
                using var input = new StringReader(text);
                using var tokenStream = analyzer.GetTokenStream(string.Empty, input);

                foreach (var token in new TokenStreamEnumerable(tokenStream))
                {
                    yield return token.ToString();
                }
            }
            finally
            {
                this.pool.Return(analyzer);
            }
        }
    }
}
