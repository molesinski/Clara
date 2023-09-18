﻿using Clara.Utils;
using Lucene.Net.Analysis.Pl;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LucenePolishAnalyzer : IAnalyzer
    {
        private readonly ObjectPool<AnalyzerContext> pool;

        public LucenePolishAnalyzer()
        {
            this.pool = new(() => new());
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

            using var context = this.pool.Lease();

            context.Instance.Reader.Reset(text);

            using var tokenStream = context.Instance.Analyzer.GetTokenStream(string.Empty, context.Instance.Reader);

            foreach (var token in new ReadOnlyTokenStreamEnumerable(tokenStream))
            {
                yield return token.ToString();
            }
        }

        private sealed class AnalyzerContext
        {
            public AnalyzerContext()
            {
                this.Reader = new ResettableStringReader();
                this.Analyzer = new PolishAnalyzer(LuceneVersion.LUCENE_48);
            }

            public ResettableStringReader Reader { get; }

            public PolishAnalyzer Analyzer { get; }
        }
    }
}
