﻿namespace Clara.Analysis
{
    public sealed class LengthKeywordTokenFilter : ITokenFilter
    {
        private readonly int minimumLength;
        private readonly int maximumLength;

        public LengthKeywordTokenFilter(int minimumLength = 1, int maximumLength = Token.MaximumLength)
        {
            if (minimumLength < 1 || minimumLength > Token.MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (maximumLength < 1 || maximumLength > Token.MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (!(minimumLength <= maximumLength))
            {
                throw new ArgumentException("Minimum length must be less or equal to maximum length.", nameof(minimumLength));
            }

            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength;
        }

        public void Process(in Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var length = token.Length;

            if (length < this.minimumLength || length > this.maximumLength)
            {
                return;
            }

            next(in token);
        }
    }
}
