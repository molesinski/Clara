using System.Text;
using Clara.Analysis.Stemming;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public class PolishMorfologikStemmer : IStemmer
    {
        private readonly ObjectPool<StemmerContext> pool;
        private readonly bool tokenOnEmptyStem;

        public PolishMorfologikStemmer(bool tokenOnEmptyStem = true)
        {
            this.pool = new(() => new());
            this.tokenOnEmptyStem = tokenOnEmptyStem;
        }

        public Token Stem(Token token)
        {
            var stemmerContext = this.pool.Get();

            try
            {
                var stemmer = stemmerContext.Stemmer;
                var input = stemmerContext.Input;
                var output = stemmerContext.Output;

                token.GetChars(out var chars);

                input.Clear();
                input.Append(chars, 0, token.Length);

                var lemmas = stemmer.Lookup(input);

                foreach (var lemma in lemmas)
                {
                    var buffer = output;
                    buffer.Clear();
                    buffer = lemma.GetStemBytes(buffer);

                    var encoding = stemmer.Dictionary.Metadata.Decoder;

                    token.Length = encoding.GetChars(buffer.Array, buffer.ArrayOffset, buffer.Limit, chars, 0);

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
                this.Input = new StringBuilder(capacity: 256);
                this.Output = ByteBuffer.Allocate(capacity: 256);
            }

            public PolishStemmer Stemmer { get; }

            public StringBuilder Input { get; }

            public ByteBuffer Output { get; }
        }
    }
}
