using Clara.Utils;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardAnalyzer : IAnalyzer
    {
        private static readonly ObjectPool<OperationContext> Pool = new(() => new());

        public IDisposableEnumerable<string> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return DisposableEnumerable<string>.Empty;
            }

            return new DisposableEnumerable<string>(GetTokensEnumerable(text));

            static IEnumerable<string> GetTokensEnumerable(string text)
            {
                using var context = Pool.Lease();

                context.Instance.Reader.Set(text);

                using var tokenStream = context.Instance.Analyzer.GetTokenStream(string.Empty, context.Instance.Reader);

                foreach (var token in new ReadOnlyTokenStreamEnumerable(tokenStream))
                {
                    yield return token.ToString();
                }
            }
        }

        private sealed class OperationContext
        {
            public OperationContext()
            {
                this.Reader = new SettableStringReader();
                this.Analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            }

            public SettableStringReader Reader { get; }

            public StandardAnalyzer Analyzer { get; }
        }
    }
}
