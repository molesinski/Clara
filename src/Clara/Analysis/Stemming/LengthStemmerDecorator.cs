using System;

namespace Clara.Analysis.Stemming
{
    public class LengthStemmerDecorator : IStemmer
    {
        private readonly IStemmer stemmer;
        private readonly int minimumLength;
        private readonly int maximumLength;

        public LengthStemmerDecorator(IStemmer stemmer, int minimumLength = 2, int maximumLength = 32)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            if (minimumLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (maximumLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (!(minimumLength <= maximumLength))
            {
                throw new ArgumentException("Minimum length must be less or equal to maximum length.", nameof(minimumLength));
            }

            this.stemmer = stemmer;
            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength;
        }

        public Token Stem(Token token)
        {
            var length = token.ValueSpan.Length;

            if (length < this.minimumLength || length > this.maximumLength)
            {
                return default;
            }

            return this.stemmer.Stem(token);
        }
    }
}
