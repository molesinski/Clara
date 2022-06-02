using System;
using Clara.Analysis.Stemming;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public class PolishMorfologikStemmer : IStemmer
    {
        private static readonly ObjectPool<StemmerBuffer> StemmerBufferPool = new(() => new());

        public Token Stem(Token token)
        {
            var stemmerBuffer = StemmerBufferPool.Get();

            try
            {
                var stemmer = stemmerBuffer.Stemmer;
                var buffer = stemmerBuffer.Buffer;
                var encoding = stemmer.Dictionary.Metadata.Decoder;

                var lemmas = stemmer.Lookup(token.ToString());

                foreach (var lemma in lemmas)
                {
                    var stemBuffer = buffer;
                    stemBuffer.Clear();
                    stemBuffer = lemma.GetStemBytes(stemBuffer);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                    var stem = new Token(StringHelper.Create(stemBuffer.Array.AsSpan(stemBuffer.ArrayOffset, stemBuffer.Limit), encoding));
#else
                    var stem = new Token(StringHelper.Create(stemBuffer.Array, stemBuffer.ArrayOffset, stemBuffer.Limit, encoding));
#endif

                    return stem;
                }

                return default;
            }
            finally
            {
                StemmerBufferPool.Return(stemmerBuffer);
            }
        }

        private sealed class StemmerBuffer
        {
            public StemmerBuffer()
            {
                this.Stemmer = new PolishStemmer();
                this.Buffer = ByteBuffer.Allocate(256);
            }

            public PolishStemmer Stemmer { get; }

            public ByteBuffer Buffer { get; }
        }
    }
}
