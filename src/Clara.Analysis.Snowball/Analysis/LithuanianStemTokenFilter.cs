﻿using Clara.Utils;
using Snowball;

namespace Clara.Analysis
{
    public sealed class LithuanianStemTokenFilter : ITokenFilter
    {
        private readonly ObjectPool<LithuanianStemmer> pool;

        public LithuanianStemTokenFilter()
        {
            this.pool = new(() => new());
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            var stemmer = this.pool.Get();

            try
            {
                var stem = stemmer.Stem(token.ToString());

                if (stem.Length > 0)
                {
                    return new Token(stem);
                }

                return default;
            }
            finally
            {
                this.pool.Return(stemmer);
            }
        }
    }
}