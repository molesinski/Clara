﻿using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public sealed class SpanishStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<SpanishStemmer> Pool = new(() => new());

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var stemmer = Pool.Lease();

            SnowballHelper.SetBufferContents(stemmer.Instance, token);

            stemmer.Instance.Stem();

            var buffer = stemmer.Instance.Buffer;

            if (buffer.Length > 0 && buffer.Length <= Token.MaximumLength)
            {
                token.Set(buffer);
            }

            return token;
        }
    }
}
