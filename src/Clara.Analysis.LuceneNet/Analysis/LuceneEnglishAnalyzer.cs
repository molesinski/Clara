﻿using Clara.Utils;
using Lucene.Net.Analysis.En;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneEnglishAnalyzer : IAnalyzer
    {
        private readonly ObjectPool<EnglishAnalyzer> pool;

        public LuceneEnglishAnalyzer()
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

            using var analyzer = this.pool.Lease();
            using var input = new StringReader(text);
            using var tokenStream = analyzer.Instance.GetTokenStream(string.Empty, input);

            foreach (var token in new TokenStreamEnumerable(tokenStream))
            {
                yield return token.ToString();
            }
        }
    }
}
