using Clara.Utils;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public class LuceneStandardTokenizer : ITokenizer
    {
        private static readonly ObjectPool<OperationContext> Pool = new(() => new());

        public IDisposableEnumerable<Token> GetTokens(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return DisposableEnumerable<Token>.Empty;
            }

            return new DisposableEnumerable<Token>(GetTokensEnumerable(text));

            static IEnumerable<Token> GetTokensEnumerable(string text)
            {
                using var context = Pool.Lease();

                context.Instance.Reader.Set(text);
                context.Instance.Tokenizer.SetReader(context.Instance.Reader);

                using var disposable = context.Instance.Tokenizer;

                foreach (var token in new TokenStreamEnumerable(context.Instance.Tokenizer, context.Instance.Chars))
                {
                    yield return token;
                }
            }
        }

        private sealed class OperationContext
        {
            public OperationContext()
            {
                this.Reader = new SettableStringReader();
                this.Tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_48, this.Reader);
                this.Chars = new char[Token.MaximumLength];
            }

            public SettableStringReader Reader { get; }

            public StandardTokenizer Tokenizer { get; }

            public char[] Chars { get; }
        }
    }
}
