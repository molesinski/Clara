using Clara.Utils;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public sealed class MorfologikStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPoolSlim<StemmerContext> Pool = new(() => new());

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var stemmerContext = Pool.Lease();

            var stemmer = stemmerContext.Instance.Stemmer;
            var input = stemmerContext.Instance.Input;
            var span = token.AsSpan();

            input.Clear();

            for (var i = 0; i < span.Length; i++)
            {
                input.Put(span[i]);
            }

            input.Flip();

            var lemmas = stemmer.Lookup(input);

            // Prefer a dictionary lemma ending in 'y' to conflate forms such as mielone/mielony and lodów/lody.
            // Reverse traversal preserves the first valid dictionary lemma as the fallback.
            // This is a search heuristic, not grammatical disambiguation; see docs/text-analysis.md.
            for (var i = lemmas.Count - 1; i >= 0; i--)
            {
                var lemma = lemmas[i];

                var buffer = stemmerContext.Instance.Output;
                buffer.Clear();
                buffer = lemma.GetStemBytes(buffer);

                var encoding = stemmer.Dictionary.Metadata.Decoder;
                var chars = stemmerContext.Instance.Chars;
                var charCount = encoding.GetChars(buffer.Array, buffer.Position, buffer.Limit, chars, 0);

                if (charCount > 0 && charCount <= Token.MaximumLength)
                {
                    token.Set(chars.AsSpan(0, charCount));

                    if (token.AsReadOnlySpan().EndsWith("y", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }
            }

            return token;
        }

        private sealed class StemmerContext
        {
            public StemmerContext()
            {
                this.Stemmer = new PolishStemmer();
                this.Input = CharBuffer.Allocate(capacity: Token.MaximumLength);
                this.Output = ByteBuffer.Allocate(capacity: Token.MaximumLength * 2);
                this.Chars = new char[Token.MaximumLength];
            }

            public PolishStemmer Stemmer { get; }

            public CharBuffer Input { get; }

            public ByteBuffer Output { get; }

            public char[] Chars { get; }
        }
    }
}
