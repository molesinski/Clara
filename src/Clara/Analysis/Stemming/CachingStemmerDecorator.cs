using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Clara.Analysis.Stemming
{
    public class CachingStemmerDecorator : IStemmer
    {
        private readonly ConcurrentDictionary<string, StemResult> cache = new();
        private readonly IStemmer stemmer;
        private readonly int maximumLength;
        private readonly int maximumCount;
        private int count;

        public CachingStemmerDecorator(IStemmer stemmer, int maximumLength = 32, int maximumCount = 16384)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            if (maximumLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumLength));
            }

            if (maximumCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumCount));
            }

            this.stemmer = stemmer;
            this.maximumLength = maximumLength;
            this.maximumCount = maximumCount;
        }

        public StemResult Stem(string token)
        {
            if (token.Length > this.maximumLength)
            {
                return this.stemmer.Stem(token);
            }

            if (this.cache.TryGetValue(token, out var stem))
            {
                return stem;
            }

            stem = this.stemmer.Stem(token);

            if (this.count < this.maximumCount)
            {
                if (this.cache.TryAdd(token, stem))
                {
                    Interlocked.Increment(ref this.count);
                }
            }

            return stem;
        }
    }
}
