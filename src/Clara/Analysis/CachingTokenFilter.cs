using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Clara.Analysis
{
    public class CachingTokenFilter : ITokenFilter
    {
        private readonly ConcurrentDictionary<Token, Token> cache = new();
        private readonly int size;
        private int count;

        public CachingTokenFilter(int size = 8192)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            this.size = size;
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (this.cache.TryGetValue(token, out var cachedStem))
            {
                return cachedStem;
            }

            if (this.count >= this.size)
            {
                return next(token);
            }

            var tokenCopy = token.ToReadOnly();
            var stemCopy = next(token).ToReadOnly();

            if (this.cache.TryAdd(tokenCopy, stemCopy))
            {
                Interlocked.Increment(ref this.count);
            }

            return stemCopy;
        }
    }
}
