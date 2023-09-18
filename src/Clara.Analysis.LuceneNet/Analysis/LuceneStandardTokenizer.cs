using Clara.Utils;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public class LuceneStandardTokenizer : ITokenizer
    {
        private readonly ObjectPool<TokenizerContext> pool;

        public LuceneStandardTokenizer()
        {
            this.pool = new(() => new());
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            using var context = this.pool.Lease();

            context.Instance.Reader.Reset(text);
            context.Instance.Tokenizer.SetReader(context.Instance.Reader);

            using var disposable = context.Instance.Tokenizer;

            foreach (var token in new TokenStreamEnumerable(context.Instance.Tokenizer, context.Instance.Chars))
            {
                yield return token;
            }
        }

        private sealed class TokenizerContext
        {
            public TokenizerContext()
            {
                this.Reader = new ResettableStringReader();
                this.Tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_48, this.Reader);
                this.Chars = new char[Token.MaximumLength];
            }

            public ResettableStringReader Reader { get; }

            public StandardTokenizer Tokenizer { get; }

            public char[] Chars { get; }
        }
    }
}
