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
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (this.cache.TryGetValue(token, out var cachedToken))
            {
                return cachedToken;
            }

            if (this.count >= this.size)
            {
                return next(token);
            }

            var inputToken = token.ToReadOnly();
            var outputToken = next(token).ToReadOnly();

            if (this.cache.TryAdd(inputToken, outputToken))
            {
                Interlocked.Increment(ref this.count);
            }

            return outputToken;
        }
    }
}
