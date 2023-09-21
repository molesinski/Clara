﻿using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public sealed class ArmenianStemTokenFilter : ITokenFilter
    {
        private static readonly ObjectPool<ArmenianStemmer> Pool = new(() => new());

        public Token Process(Token token, TokenFilterDelegate next)
        {
            using var stemmer = Pool.Lease();

            var stem = stemmer.Instance.Stem(token.ToString());

            if (stem.Length > 0)
            {
                return new Token(stem);
            }

            return default;
        }
    }
}