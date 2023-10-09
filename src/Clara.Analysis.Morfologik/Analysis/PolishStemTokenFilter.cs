using System.Text;
using Clara.Utils;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public sealed class PolishStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<StemmerContext> Pool = new(() => new());

        public void Process(in Token token, TokenFilterDelegate next)
        {
            using var stemmerContext = Pool.Lease();

            var stemmer = stemmerContext.Instance.Stemmer;
            var input = stemmerContext.Instance.Input;
            var output = stemmerContext.Instance.Output;
            var chars = stemmerContext.Instance.Chars;

            input.Clear();
            token.CopyTo(input);

            var lemmas = stemmer.Lookup(input);

            foreach (var lemma in lemmas)
            {
                var buffer = output;
                buffer.Clear();
                buffer = lemma.GetStemBytes(buffer);

                var encoding = stemmer.Dictionary.Metadata.Decoder;
                var charCount = encoding.GetChars(buffer.Array, buffer.ArrayOffset, buffer.Limit, chars, 0);

                if (charCount > 0 && charCount <= Token.MaximumLength)
                {
                    token.Set(chars.AsSpan(0, charCount));
                }

                break;
            }
        }

        private sealed class StemmerContext
        {
            public StemmerContext()
            {
                this.Stemmer = new PolishStemmer();
                this.Input = new StringBuilder(capacity: Token.MaximumLength);
                this.Output = ByteBuffer.Allocate(capacity: Token.MaximumLength * 2);
                this.Chars = new char[Token.MaximumLength];
            }

            public PolishStemmer Stemmer { get; }

            public StringBuilder Input { get; }

            public ByteBuffer Output { get; }

            public char[] Chars { get; }
        }
    }
}
