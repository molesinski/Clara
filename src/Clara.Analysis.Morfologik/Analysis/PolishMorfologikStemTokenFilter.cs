using System;
using System.Text;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public class PolishMorfologikStemTokenFilter : ITokenFilter
    {
        private readonly ObjectPool<StemmerContext> pool;
        private readonly bool tokenOnEmptyStem;

        public PolishMorfologikStemTokenFilter(bool tokenOnEmptyStem = true)
        {
            this.pool = new(() => new());
            this.tokenOnEmptyStem = tokenOnEmptyStem;
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            var stemmerContext = this.pool.Get();

            try
            {
                var stemmer = stemmerContext.Stemmer;
                var input = stemmerContext.Input;
                var output = stemmerContext.Output;
                var chars = stemmerContext.Chars;

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

                    token.Set(chars.AsSpan(0, charCount));

                    return token;
                }

                if (this.tokenOnEmptyStem)
                {
                    return token;
                }

                return default;
            }
            finally
            {
                this.pool.Return(stemmerContext);
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
