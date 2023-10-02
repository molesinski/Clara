using System.Collections.Concurrent;

namespace Clara.Analysis
{
    public sealed class CachingTokenFilter : ITokenFilter
    {
        private readonly ConcurrentDictionary<Token, Token> cache = new();
        private readonly int maximumCapacity;
        private int count;

        public CachingTokenFilter(int maximumCapacity = 8192)
        {
            if (maximumCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumCapacity));
            }

            this.maximumCapacity = maximumCapacity;
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

            if (this.count >= this.maximumCapacity)
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
