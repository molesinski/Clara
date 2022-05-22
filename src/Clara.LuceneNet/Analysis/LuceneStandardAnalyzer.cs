using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardAnalyzer : ITokenizer
    {
        private static readonly ThreadLocal<StandardAnalyzer> Analyzer = new(() => new StandardAnalyzer(LuceneVersion.LUCENE_48));

        private readonly IStringFactory stringFactory;

        public LuceneStandardAnalyzer()
            : this(DefaultStringFactory.Instance)
        {
        }

        public LuceneStandardAnalyzer(IStringFactory stringFactory)
        {
            if (stringFactory is null)
            {
                throw new ArgumentNullException(nameof(stringFactory));
            }

            this.stringFactory = stringFactory;
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

            var analyzer = Analyzer.Value!;

            using (var input = new StringReader(text))
            {
                using (var tokenStream = analyzer.GetTokenStream(string.Empty, input))
                {
                    foreach (var token in new TokenStreamEnumerable(tokenStream, this.stringFactory))
                    {
                        yield return token;
                    }
                }
            }
        }
    }
}
