using System;
using System.Collections.Generic;
using System.Threading;
using J2N.IO;
using Morfologik.Stemming.Polish;

namespace Clara.Analysis
{
    public class MorfologikStemTokenFilter : ITokenFilter
    {
        private static readonly ThreadLocal<PolishStemmer> Stemmer = new(() => new PolishStemmer());
        private static readonly ThreadLocal<ByteBuffer> Buffer = new(() => ByteBuffer.Allocate(256));

        private readonly IStringFactory stringFactory;

        public MorfologikStemTokenFilter()
            : this(DefaultStringFactory.Instance)
        {
        }

        public MorfologikStemTokenFilter(IStringFactory stringFactory)
        {
            if (stringFactory is null)
            {
                throw new ArgumentNullException(nameof(stringFactory));
            }

            this.stringFactory = stringFactory;
        }

        public IEnumerable<string> Filter(IEnumerable<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            var stemmer = Stemmer.Value!;
            var buffer = Buffer.Value!;
            var encoding = stemmer.Dictionary.Metadata.Decoder;

            foreach (var token in tokens)
            {
                var hadStems = false;
                var lemmas = stemmer.Lookup(token);

                foreach (var lemma in lemmas)
                {
                    var stemBuffer = buffer;
                    stemBuffer.Clear();
                    stemBuffer = lemma.GetStemBytes(stemBuffer);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                    yield return this.stringFactory.Create(stemBuffer.Array.AsSpan(stemBuffer.ArrayOffset, stemBuffer.Limit), encoding);
#else
                    yield return this.stringFactory.Create(stemBuffer.Array, stemBuffer.ArrayOffset, stemBuffer.Limit, encoding);
#endif
                }

                if (!hadStems)
                {
                    yield return token;
                }
            }
        }
    }
}
