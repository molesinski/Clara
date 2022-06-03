using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Clara.Analysis
{
    public class CachingStemmerDecorator : IStemmer
    {
        private readonly ConcurrentDictionary<Token, Token> cache = new();
        private readonly IStemmer stemmer;
        private readonly int maximumCount;
        private int count;

        public CachingStemmerDecorator(IStemmer stemmer, int maximumCount = 8192)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            if (maximumCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumCount));
            }

            this.stemmer = stemmer;
            this.maximumCount = maximumCount;
        }

        public Token Stem(Token token)
        {
            if (this.cache.TryGetValue(token, out var cachedStem))
            {
                return cachedStem;
            }

            if (this.count >= this.maximumCount)
            {
                return this.stemmer.Stem(token);
            }

            var tokenCopy = token.ToReadOnly();
            var stemCopy = this.stemmer.Stem(token).ToReadOnly();

            if (this.cache.TryAdd(tokenCopy, stemCopy))
            {
                Interlocked.Increment(ref this.count);
            }

            return stemCopy;
        }
    }
}
