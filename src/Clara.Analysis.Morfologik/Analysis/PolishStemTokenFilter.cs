using Clara.Utils;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public sealed class PolishStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<StemmerContext> Pool = new(() => new());

        public void Process(ref Token token, TokenFilterDelegate next)
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

            if (lemmas.Count > 0)
            {
                var lemma = lemmas[0];

                var buffer = stemmerContext.Instance.Output;
                buffer.Clear();
                buffer = lemma.GetStemBytes(buffer);

                var encoding = stemmer.Dictionary.Metadata.Decoder;
                var chars = stemmerContext.Instance.Chars;
                var charCount = encoding.GetChars(buffer.Array, buffer.ArrayOffset, buffer.Limit, chars, 0);

                if (charCount > 0 && charCount <= Token.MaximumLength)
                {
                    token.Set(chars.AsSpan(0, charCount));
                }
            }
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
